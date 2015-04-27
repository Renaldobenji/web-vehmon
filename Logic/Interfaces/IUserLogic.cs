using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Contracts.UserCreation;

namespace Logic
{
    /// <summary>
    /// Main interface for the logic of user management
    /// </summary>
    public interface IUserLogic
    {
        List<UserDetailContract> GetAllUsers(Guid token);
        
        UserCreationResponse CreateUser(UserDetailContract userDetails);

        TokenGenerationResult GetTokenForUser(string userName, string password);

        UserTokenValidationResponse ValidateToken(String userName,Guid token);

        UserTokenValidationResponse ValidateToken(Guid token);

        UserTokenValidationResponse ValidateTokenInRole(Guid token, String role);

        UserTokenValidationResponse RenewToken(Guid token);

        //Not used by API only by example page

        List<CompanyResponse> GetCompanies();

        int CreateCompany(string companyName);

        UserCreationResponse SetDeviceId(string token, string deviceId);
    }
}
