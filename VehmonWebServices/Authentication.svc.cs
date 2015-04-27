using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Logic.Contracts.UserCreation;
using Logic;
namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Authentication" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Authentication.svc or Authentication.svc.cs at the Solution Explorer and start debugging.
    //[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Authentication : IAuthenticationServiceContract
    {
        private IUserLogic _userLogic;
        public Authentication()
        {
            _userLogic = new UserLogic();
        }

        public UserTokenValidationResponse RenewToken(string token)
        {
            return _userLogic.RenewToken(Guid.Parse(token));
        }

        public TokenGenerationResult GetTokenForUser(string userName, string password)
        {
            return _userLogic.GetTokenForUser(userName, password);
        }

        public UserTokenValidationResponse IsTokenValid(string userName,string token)
        {
            return _userLogic.ValidateToken(userName,Guid.Parse(token));
        }

        public UserCreationResponse CreateUser(string userName, string password, string title, string firstName, string surname,
            string employerNumber, string identificationNumber, string deviceId, string emailAddress, string cellNumber)
        {
            return _userLogic.CreateUser(new UserDetailContract
            {
                UserName = userName,
                Password = password,
                Title = title,
                FirstName = firstName,
                LastName = surname,
                EmployerNumber = employerNumber,
                IdNumber = identificationNumber,
                DeviceId = deviceId,
                EmailAddress = emailAddress,
                CellPhoneNumber = cellNumber,
            });
        }

        public UserTokenValidationResponse ValidateTokenInRole(string token, string role)
        {
            return _userLogic.ValidateTokenInRole(Guid.Parse(token), role);
        }

        public List<UserDetailContract> GetAllUsers(string token)
        {
            return _userLogic.GetAllUsers(Guid.Parse(token));
        }

        public UserCreationResponse SetDeviceId(string token, string deviceId)
        {
            return _userLogic.SetDeviceId(token,deviceId);
        }

        public void DoWork()
        {
        }
    }
}
