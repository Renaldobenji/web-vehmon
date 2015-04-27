using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Logic;
using Logic.Contracts.UserCreation;

namespace VehmonWebServices
{
    [ServiceContract]
    public interface IAuthenticationServiceContract
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "RenewToken/{token}", ResponseFormat = WebMessageFormat.Json)]
        UserTokenValidationResponse RenewToken(String token);

        [OperationContract]
        [WebGet(UriTemplate = "GetTokenForUser/{userName}/{password}",ResponseFormat=WebMessageFormat.Json)]
        TokenGenerationResult GetTokenForUser(String userName, String password);

        [OperationContract]
        [WebGet(UriTemplate = "TokenValidate/{userName}/{token}", ResponseFormat = WebMessageFormat.Json)]
        UserTokenValidationResponse IsTokenValid(String userName,String token);

        [OperationContract]
        [WebInvoke(UriTemplate = "CreateUser/{userName}/{password}/{title}/{firstName}/{surname}/{employerNumber}/{identificationNumber}/{deviceId}/{emailAddress}/{cellNumber}", ResponseFormat = WebMessageFormat.Json)]
        UserCreationResponse CreateUser(String userName, String password, String title, String firstName, String surname, String employerNumber, String identificationNumber, String deviceId, String emailAddress, String cellNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "ValidateTokenInRole/{token}/{role}", ResponseFormat = WebMessageFormat.Json)]
        UserTokenValidationResponse ValidateTokenInRole(string token, string role);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllUsers/{token}", ResponseFormat = WebMessageFormat.Json)]
        List<UserDetailContract> GetAllUsers(string token);

        [OperationContract]
        [WebGet(UriTemplate = "SetDeviceId/{token}/{deviceId}", ResponseFormat = WebMessageFormat.Json)]
        UserCreationResponse SetDeviceId(string token, String deviceId);

        [OperationContract]
        void DoWork();
    }
}
