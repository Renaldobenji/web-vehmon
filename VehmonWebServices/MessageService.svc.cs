using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logic;
using Logic.Contracts.Messaging;
using Logic.Interfaces;

namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessageService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageService.svc or MessageService.svc.cs at the Solution Explorer and start debugging.
   // [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]//
    public class MessageService : IMessageServiceContract
    {
        private IMessagingLogic _messageLogic;

        public MessageService()
        {
            _messageLogic = new MessageLogic();
        }

        public ConversationResponse CreateConversation(string token, string conversationName, string userNames)
        {
            return _messageLogic.CreateConversation(Guid.Parse(token), conversationName, userNames);
        }

        public List<ConversationResponse> GetAllUserConversations(string token)
        {
            return _messageLogic.GetAllUserConversations(Guid.Parse(token));
        }


        public List<MessageResponse> GetAllMessagesForConversation(string token, string conversationId, string lastCount)
        {
            return _messageLogic.GetAllMessagesForConversation(Guid.Parse(token), int.Parse(conversationId), int.Parse(lastCount));
        }

        public List<MessageResponse> GetAllUnreadMessages(string token)
        {
            return _messageLogic.GetAllUnreadMessages(Guid.Parse(token));
        }

        public List<MessageResponse> GetAllUnreadMessagesForConversation(string token, string conversationId)
        {
            return _messageLogic.GetAllUnreadMessagesForConversation(Guid.Parse(token), int.Parse(conversationId));
        }

        public RemoveConversationResponse RemoveConversation(string token, string conversationId)
        {
            return _messageLogic.RemoveConversation(Guid.Parse(token),int.Parse( conversationId));
        }

        public MessageResponse SendMessage(string token, string conversationId, string dateSent, string message)
        {
            return _messageLogic.SendMessage(Guid.Parse(token), new SendMessageRequest
            {
                ConversationId = int.Parse(conversationId),
                Message = message,
                MessageSentDate = DateTime.ParseExact(dateSent, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture),
            });

        }

        public List<UserDetailContract> GetAllUsersForConversation(string token, string conversationId)
        {
            return _messageLogic.GetAllUsersForConversation(Guid.Parse(token), int.Parse(conversationId));
        }

        public MessageResponse AddUserToConversation(string token, string conversationId, string userName)
        {
            return _messageLogic.AddUserToConversation(Guid.Parse(token), int.Parse(conversationId), userName);
        }

        public void DoWork()
        {
        }
    }
}
