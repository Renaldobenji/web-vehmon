using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebDAL;
using WehmonWeb.Models;

namespace WehmonWeb.Hubs
{
    public class ChatHub : Hub
    {
        private static List<UserInfo> UsersList = new List<UserInfo>();
        public List<ConversationUser> ConnectedUsers = new List<ConversationUser>();

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients
            Clients.All.addNewMessageToPage(name, message);
        }


        public void ConnectUser(string userName)
        {
            if (UsersList.Any(x => x.UserName == userName))
                return;

            var id = Context.ConnectionId;
            using (var context = new vehmonEntities())
            {
                var user = context.users.Single(x => x.username == userName);
                var userGroup = user.company.companyName;

                UsersList.Add(new UserInfo
                {
                    ConnectionId = id,
                    UserID = user.userID,
                    UserName = userName,
                    UserGroup = userGroup,
                    freeflag = "0",
                    tpflag = "0",
                });

                Groups.Add(Context.ConnectionId, userGroup);
                //create a group for each chat
                var userConversations = user.userconversations.Select(x => x.conversationID).ToList();
                Clients.Caller.onConnected(id, userName, user.userID, "Admin");
            }
        }

        public void SendMessageToGroup(string userName, string message, string group)
        {
            using (var context = new vehmonEntities())
            {
                var id = int.Parse(group);
                var chat = context.conversations.Single(x => x.conversationID == id);
                var users = chat.userconversations.Select(x => x.user);
                foreach (var user in users)
                {
                    var client = UsersList.FirstOrDefault(x => x.UserName == user.username);
                    if (client != null)
                    {
                        Clients.Client(client.ConnectionId).getMessages(userName, message, group);
                    }
                }
            }

        }

        // <<<<<-- ***** Return to Client [  getMessages  ] *****

        //--group ***** Receive Request From Client ***** 
        //{ Whenever User close session then OnDisconneced will be occurs }
        public override System.Threading.Tasks.Task OnDisconnected()
        {

            var item = UsersList.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                UsersList.Remove(item);

                var id = Context.ConnectionId;

                if (item.tpflag == "0")
                {
                    //user logged off == user
                    try
                    {
                        var stradmin = (from s in UsersList
                                        where
                                            (s.UserGroup == item.UserGroup) && (s.tpflag == "1")
                                        select s).First();
                        //become free
                        stradmin.freeflag = "1";
                    }
                    catch
                    {
                        //***** Return to Client *****
                        Clients.Caller.NoExistAdmin();
                    }
                }

                //save conversation to dat abase
            }

            return base.OnDisconnected();
        }

        public void SendMessageToAll(string userName, string message)
        {

        }

        public void SendPrivateMessage(string toUserId, string message)
        {

        }

    }
}