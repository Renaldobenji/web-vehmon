using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Logic.Contracts.Messaging;
using Logic.HelperLogic;
using Logic.Interfaces;

namespace Logic
{
    public class MessageLogic : IMessagingLogic
    {
        public List<ConversationResponse> GetAllUserConversations(Guid token)
        {
            List<ConversationResponse> conversationResponses = new List<ConversationResponse>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    return conversationResponses;
                }
                var conversations = userCurrent.userconversations.Where(x => x.isHidden == false).ToList();
                foreach (var userconversation in conversations)
                {
                    conversationResponses.Add(new ConversationResponse
                    {
                        ConversationId = userconversation.conversation.conversationID,
                        ConversationName = userconversation.conversation.name
                    });
                }
            }
            return conversationResponses;
        }

        public List<MessageResponse> GetAllMessagesForConversation(Guid token, int conversationId, int lastCount = -1)
        {
            List<MessageResponse> conversationResponses = new List<MessageResponse>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    return conversationResponses;
                }
                int curUserId = userCurrent.userID;
                var messageRecps = context.usermessagereceipts.Where(u => u.message.conversationID == conversationId && u.userID == curUserId).Include(x => x.message).Include(x => x.message.user).OrderByDescending(x=>x.message.dateSent).ToList();
                foreach (var userconversation in messageRecps)
                {
                    conversationResponses.Add(new MessageResponse
                    {
                        Message = userconversation.message.messageText,
                        Sender = userconversation.message.user.username,
                        ConversationId = userconversation.message.conversationID,
                        HasReceived = userconversation.hasReceived ?? false
                    });
                    userconversation.hasReceived = true;
                }
                context.SaveChanges();
            }

            return conversationResponses; 
        }

        public List<MessageResponse> GetAllUnreadMessages(Guid token)
        {
            List<MessageResponse> conversationResponses = new List<MessageResponse>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    return conversationResponses;
                }
                var messageRecps = context.usermessagereceipts.Where(u => u.hasReceived == false && u.userID == userCurrent.userID ).Include(x => x.message).Include(x => x.message.user).OrderByDescending(x=>x.message.dateSent).ToList();
                foreach (var userconversation in messageRecps)
                {
                    userconversation.hasReceived = true;
                    conversationResponses.Add(new MessageResponse
                    {
                        Message = userconversation.message.messageText,
                        Sender = userconversation.message.user.username,
                        ConversationId = userconversation.message.conversationID,
                        HasReceived = userconversation.hasReceived ?? false
                    });
                }
                context.SaveChanges();
            }

            return conversationResponses; 
        }

        public List<MessageResponse> GetAllUnreadMessagesForConversation(Guid token, int conversationId)
        {
            List<MessageResponse> conversationResponses = new List<MessageResponse>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    return conversationResponses;
                }

                var messageRecps = context.usermessagereceipts.Where(u => u.hasReceived == false && u.userID ==userCurrent.userID && u.message.conversationID == conversationId).Include(x => x.message).Include(x=>x.message.user).OrderByDescending(x=>x.message.dateSent).ToList();
                foreach (var userconversation in messageRecps)
                {
                    userconversation.hasReceived = true;
                    conversationResponses.Add(new MessageResponse
                    {
                        Message = userconversation.message.messageText,
                        Sender = userconversation.message.user.username,
                        ConversationId = userconversation.message.conversationID,
                        HasReceived = userconversation.hasReceived ?? false
                    });
                }
                context.SaveChanges();
            }
            
            return conversationResponses; 
        }

        public RemoveConversationResponse RemoveConversation(Guid token, int conversationId)
        {
            RemoveConversationResponse conversationResponses = new RemoveConversationResponse();
            conversationResponses.RemoveStatus = RemoveConversationStatus.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    conversationResponses.RemoveStatus = RemoveConversationStatus.ConversationNotFound;
                    return conversationResponses;
                }
                var conversations = userCurrent.userconversations.Single(x => x.conversationID == conversationId);
                conversations.isHidden = true;
                context.SaveChanges();
            }
            return conversationResponses;
        }

        public MessageResponse SendMessage(Guid token, SendMessageRequest messageRequest)
        {
            MessageResponse messageResponses = new MessageResponse();
            messageResponses.MessageStatus = SendMessageStatus.Successfull;
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    messageResponses.MessageStatus = SendMessageStatus.Failed;
                    return messageResponses;
                }
                var conversations = userCurrent.userconversations.Single(x => x.conversationID == messageRequest.ConversationId);
                var allUsersForConversation = conversations.conversation.userconversations.Select(x => x.user).ToList();
                var newMessage = new message
                {
                    dateSent = messageRequest.MessageSentDate,
                    messageText = messageRequest.Message,
                    user = userCurrent
                };

                foreach (var user in allUsersForConversation)
                {
                    var userMessagee = new usermessagereceipt
                    {
                        hasReceived = user.userID == userCurrent.userID,
                        message = newMessage,
                        user = user
                    };
                    context.usermessagereceipts.Add(userMessagee);
                }

                conversations.conversation.messages.Add(newMessage);

                context.SaveChanges();

                foreach (var users in allUsersForConversation)
                {
                    var user = context.users.FirstOrDefault(x => x.userID == users.userID);
                    if (user != null && user.userID != userCurrent.userID)
                    {
                        //Push Notificaiton to user
                        new AndroidPushNotifications().PushNotification(
                                user.deviceID
                                , new Contracts.Notifications.VehmonNotification()
                                {
                                    NotificationType = "MessageReceived"
                                    ,
                                    NotificationPayload = "This is the message Payload"
                                }
                            );
                    }
                }

                return messageResponses;
            }
        }

        public List<UserDetailContract> GetAllUsersForConversation(Guid token, int conversationId)
        {
            List<UserDetailContract> messageResponses = new List<UserDetailContract>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    return messageResponses;
                }
                var conversation = context.conversations.FirstOrDefault(x=>x.conversationID == conversationId);
                var users = conversation.userconversations.Select(x => x.user);

                foreach (var user in users)
                {
                    messageResponses.Add(new UserDetailContract
                    {
                        CellPhoneNumber = user.cellNumber,
                        DeviceId = user.deviceID,
                        UserID = user.userID,
                        EmailAddress = user.emailAddress,
                        EmployerNumber = user.employerNumber,
                        FirstName = user.firstname,
                        LastName = user.surname,
                        IdNumber = user.identificationNumber,
                        Title = user.title,
                        UserName = user.username
                    });
                }
            }
            return messageResponses;
        }

        public MessageResponse AddUserToConversation(Guid token, int conversationId, string userName)
        {
            MessageResponse messageResponses = new MessageResponse();
            messageResponses.MessageStatus = SendMessageStatus.Successfull;
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    messageResponses.MessageStatus = SendMessageStatus.Failed;
                    return messageResponses;
                }
                var conversations = userCurrent.userconversations.Single(x => x.conversationID == conversationId);

                var userToAdd = context.users.FirstOrDefault(x => x.username == userName);

                if (userToAdd == null)
                {
                    messageResponses.MessageStatus = SendMessageStatus.UserNotFound;
                    return messageResponses;
                }

                var userConversation = new userconversation
                {
                    conversation = conversations.conversation,
                    user = userToAdd,
                    isHidden = false,

                };

                context.userconversations.Add(userConversation);
                context.SaveChanges();
                return messageResponses;
            }
        }

        public ConversationResponse CreateConversation(Guid token,string conversationName ,string usernames)
        {
            ConversationResponse messageResponses = new ConversationResponse();
            messageResponses.CreateStatus = ConversationStatus.Successfull;
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {
                    messageResponses.CreateStatus = ConversationStatus.Failed;
                    return messageResponses;
                }
                var newConversation = new conversation
                {
                    dateCreated = DateTime.Now,
                    name = conversationName,
                };
                context.conversations.Add(newConversation);
                var conversation = new userconversation
                {
                    conversation = newConversation,
                    isHidden = false,
                    user = userCurrent
                };
                userCurrent.userconversations.Add(conversation);
                context.SaveChanges();
                messageResponses.ConversationId = conversation.conversationID;
                foreach (var username in usernames.Split(","[0]))
                {
                    var user = context.users.FirstOrDefault(x => x.username == username);
                    if (user == null)
                        continue;
                    var conversationtmp = new userconversation
                    {
                        conversation = newConversation,
                        isHidden = false,
                        user = user
                    };
                    context.userconversations.Add(conversationtmp);
                  
                }
                context.SaveChanges();
                
                return messageResponses;
            }
        }
    }
}
