using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebDAL;
using WehmonWeb.Models;

namespace WehmonWeb.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        public ActionResult RenderConversationPartial()
        {
            var model = new ChatModel();
            model.ConversationModels = new List<ConversationModel>();
            using (var context = new vehmonEntities())
            {

                var userName = HttpContext.User.Identity.Name;
                var currentUser =
                    context.users.Include("userconversations")
                        .Include("userconversations.conversation")
                        .Include("userconversations.conversation.messages")
                        .Single(x => x.username == userName);
                foreach (var userconversation in currentUser.userconversations.Where(x => !x.isHidden))
                {
                    var conversationUsers = userconversation.conversation.userconversations.Select(x => x.user).ToList();
                    var conversationModel = new ConversationModel
                    {
                        Users =
                            conversationUsers.Select(
                                x => new ConversationUser { UserId = x.userID, UserName = x.username }).ToList(),
                        ConversationName = userconversation.conversation.name,
                        ID = userconversation.conversation.conversationID,
                        Messages =
                            userconversation.conversation.messages.OrderBy(x => x.dateSent).Select(x => new MessageModel
                            {
                                Message = x.messageText,
                                Sender = conversationUsers.First(u => x.userID == u.userID).username,
                                Time = x.dateSent
                            }).ToList()
                    };
                    model.ConversationModels.Add(conversationModel);
                }
                model.UserList = currentUser.company.users.ToList().Select(x => new ConversationUser
                {
                    UserId = x.userID,
                    UserName = x.username
                });
                //var conversations = currentUser.userconversations.Where(x=>x.isHidden == false).Select(x=>x.conversation.messages)
            }
            return PartialView("_ConversationPartial", model);
        }

        public ActionResult RenderUserPartial(int conversationId)
        {

            var model = new ConversationUserModel();
            using (var context = new vehmonEntities())
            {
                var userName = HttpContext.User.Identity.Name;
                var currentUser = context.users.Single(x => x.username == userName);

                model.UserList = currentUser.company.users.ToList().Select(x => new ConversationUser
                {
                    UserId = x.userID,
                    UserName = x.username
                });

                model.ConversationUserList =
                    context.conversations.Single(x => x.conversationID == conversationId)
                        .userconversations.Select(u => u.user)
                        .ToList()
                        .Select(x => new ConversationUser
                        {
                            UserId = x.userID,
                            UserName = x.username
                        }).ToList();
            }
            return PartialView("_ConversationUserPartial", model);
        }

        [HttpPost]
        public JsonResult SetConversationUsers(int conversationId, int userId)
        {
            using (var context = new vehmonEntities())
            {
                var conversation = context.conversations.Single(x => x.conversationID == conversationId);

                conversation.userconversations.Add(new userconversation
                {
                    isHidden = false,
                    user = context.users.Single(x => x.userID == userId),
                });
                context.SaveChangesAsync();

            }

            return Json(new { success = true });
        }

        public JsonResult RemoveChat(int converstationId)
        {
            using (var context = new vehmonEntities())
            {
                var userName = HttpContext.User.Identity.Name;
                var currentUser = context.users.Single(x => x.username == userName);
                var conversations = currentUser.userconversations.Single(x => x.conversationID == converstationId);
                conversations.isHidden = true;
                context.SaveChanges();

            }
            return Json(new { success = true });

        }

        [HttpPost]
        public JsonResult SendUserMessage(int converstationId, string message)
        {
            using (var context = new vehmonEntities())
            {
                var userName = HttpContext.User.Identity.Name;
                var currentUser = context.users.Single(x => x.username == userName);

                var conversations = currentUser.userconversations.Single(x => x.conversationID == converstationId);
                var allUsersForConversation = conversations.conversation.userconversations.Select(x => x.user).ToList();
                var newMessage = new message
                {
                    dateSent = DateTime.Now,
                    messageText = message,
                    user = currentUser
                };

                foreach (var user in allUsersForConversation)
                {
                    var userMessagee = new usermessagereceipt
                    {
                        hasReceived = user.userID == currentUser.userID,
                        message = newMessage,
                        user = user
                    };
                    context.usermessagereceipts.Add(userMessagee);
                }

                conversations.conversation.messages.Add(newMessage);

                context.SaveChanges();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult AddConversation(string conversationName)
        {
            using (var context = new vehmonEntities())
            {
                var userName = HttpContext.User.Identity.Name;
                var currentUser = context.users.Single(x => x.username == userName);

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
                    user = currentUser
                };
                currentUser.userconversations.Add(conversation);
                context.SaveChanges();

                return Json(new { success = true });
            }

        }

        public ActionResult RenderChatFieldAjax(int number, string sender, string message)
        {
            ViewBag.Sender = sender;
            ViewBag.Message = message;

            ViewBag.TimeSent = DateTime.Now;

            if (number % 2 == 0)
            {
                return PartialView("_chatRight");
            }
            else
            {
                return PartialView("_chatLeft");
            }
        }

        public ActionResult RenderChatField(int number, DateTime time, string sender, string message, bool useNow = false)
        {
            ViewBag.TimeSent = time;
            ViewBag.Sender = sender;
            ViewBag.Message = message;
            if (useNow)
            {
                ViewBag.TimeSent = DateTime.Now;
            }
            if (number % 2 == 0)
            {
                return PartialView("_chatRight");
            }
            else
            {
                return PartialView("_chatLeft");
            }
        }
    }
}
