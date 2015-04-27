using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Contracts.Messaging;

namespace Logic.Interfaces
{
    public interface IMessagingLogic
    {
        List<ConversationResponse> GetAllUserConversations(Guid token);

        List<MessageResponse> GetAllMessagesForConversation(Guid token, int conversationId,int lastCount = -1);

        List<MessageResponse> GetAllUnreadMessages(Guid token);

        List<MessageResponse> GetAllUnreadMessagesForConversation(Guid token, int conversationId);

        RemoveConversationResponse RemoveConversation(Guid token, int conversationId);

        MessageResponse SendMessage(Guid token, SendMessageRequest messageRequest);

        List<UserDetailContract> GetAllUsersForConversation(Guid token, int conversationId);

        MessageResponse AddUserToConversation(Guid token, int conversationId, string userName);

        ConversationResponse CreateConversation(Guid parse, string conversationName, string usernames);
    }
}
