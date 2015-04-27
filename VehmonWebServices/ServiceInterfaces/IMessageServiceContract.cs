using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Logic;
using Logic.Contracts.Messaging;
using Logic.Contracts.UserCreation;

namespace VehmonWebServices
{
    [ServiceContract]
    public interface IMessageServiceContract
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "CreateConversation/{token}/{conversationName}/{userNames}", ResponseFormat = WebMessageFormat.Json)]
        ConversationResponse CreateConversation(string token, string conversationName, string userNames);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllUserConversations/{token}", ResponseFormat = WebMessageFormat.Json)]
        List<ConversationResponse> GetAllUserConversations(String token);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllMessagesForConversation/{token}/{conversationId}/{lastCount}", ResponseFormat = WebMessageFormat.Json)]
        List<MessageResponse> GetAllMessagesForConversation(String token, string conversationId, String lastCount);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllUnreadMessages/{token}", ResponseFormat = WebMessageFormat.Json)]
        List<MessageResponse> GetAllUnreadMessages(String token);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllUnreadMessagesForConversation/{token}/{conversationId}", ResponseFormat = WebMessageFormat.Json)]
        List<MessageResponse> GetAllUnreadMessagesForConversation(string token, String conversationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "RemoveConversation/{token}/{conversationId}", ResponseFormat = WebMessageFormat.Json)]
        RemoveConversationResponse RemoveConversation(string token, String conversationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "SendMessage/{token}/{conversationId}/{dateSent}/{message}", ResponseFormat = WebMessageFormat.Json)]
        MessageResponse SendMessage(string token, String conversationId, string dateSent, string message);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllUsersForConversation/{token}/{conversationId}", ResponseFormat = WebMessageFormat.Json)]
        List<UserDetailContract> GetAllUsersForConversation(string token, String conversationId);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddUserToConversation/{token}/{conversationId}/{userName}", ResponseFormat = WebMessageFormat.Json)]
        MessageResponse AddUserToConversation(string token, String conversationId, string userName);

        [OperationContract]
        void DoWork();
    }
}
