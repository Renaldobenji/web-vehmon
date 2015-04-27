using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Logic.Contracts.UserCreation;

namespace Logic
{
    public class UserLogic : IUserLogic
    {
        private const int TokenValidMinitues = 120;
        public List<UserDetailContract> GetAllUsers(Guid token)
        {
            using (var vehmonEntities = new vehmonEntities2())
            {
                var usercompany = vehmonEntities.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (usercompany == null)
                    return new List<UserDetailContract>();
                return usercompany.user.company.users.ToList().Select(x => new UserDetailContract()
                {
                    CellPhoneNumber = x.cellNumber,
                    DeviceId = x.deviceID,
                    EmailAddress = x.emailAddress,
                    EmployerNumber = x.employerNumber,
                    FirstName = x.firstname,
                    IdNumber = x.identificationNumber,
                    LastName = x.surname,
                    Title = x.title,
                    UserID = x.userID,
                    UserName = x.username
                }).ToList();
            }
        }

        public UserCreationResponse CreateUser(UserDetailContract userDetails)
        {
            var passwordHasher = new PasswordHasher();
            var response = new UserCreationResponse();
            response.IsSuccess = true;
            using (var vehmonEntities = new vehmonEntities2())
            {
                if (vehmonEntities.users.Any(x => x.username == userDetails.UserName))
                {
                    response.ErrorMessage = "Username allready exists.";
                    response.IsSuccess = false;
                    return response;
                }
                if (userDetails.Password.Length < 6)
                {
                    response.ErrorMessage = "Password is too short";
                    response.IsSuccess = false;
                    return response;
                }
                var company = vehmonEntities.companies.FirstOrDefault(x => x.companyID == userDetails.CompanyID);
                if (company == null)
                {
                    company = new company
                    {
                        companyName = "Vehmon",
                        dateAdded = DateTime.Now,
                        isActive = true,
                    };
                }
                string saltValue = "";
                var passwordHash = passwordHasher.HashPassword(userDetails.Password, ref saltValue, new HMACMD5());
                var newUser = new user
                {
                    company = company,
                    cellNumber = userDetails.CellPhoneNumber,
                    employerNumber = userDetails.EmployerNumber,
                    surname = userDetails.LastName,
                    title = userDetails.Title,
                    username = userDetails.UserName,
                    updated = DateTime.Now,
                    created = DateTime.Now,
                    emailAddress = userDetails.EmailAddress,
                    deviceID = "4554",
                    passwordSalt = saltValue,
                    password = passwordHash,
                    firstname = userDetails.FirstName,
                    identificationNumber = userDetails.IdNumber,
                    isApproved = true,
                    lastActivityDate = DateTime.Now,
                    lastPasswordChange = DateTime.Now,
                    isLockedOut = "false",
                    failedPasswordAttemptCount = 0,
                    lastLoginDate = DateTime.Now
                };
                vehmonEntities.users.Add(newUser);
                vehmonEntities.SaveChanges();
                response.NewUserId = newUser.userID;
            };
            return response;
        }

        public TokenGenerationResult GetTokenForUser(string userName, string password)
        {
            var passwordHasher = new PasswordHasher();
            var response = new TokenGenerationResult();
            response.TokenGenerationState = TokenGenerationState.Valid;
            using (var vehmonEntities = new vehmonEntities2())
            {
                var user = vehmonEntities.users.FirstOrDefault(x => x.username == userName);
                if (user == null)
                {
                    response.TokenGenerationState = TokenGenerationState.UserNotFound;
                    return response;
                }
                var valid = passwordHasher.ValidatePassword(password, user.passwordSalt, user.password);
                if (!valid)
                {
                    response.TokenGenerationState = TokenGenerationState.InvalidPassword;
                }
                else
                {
                    var token = user.authenticationtokens.FirstOrDefault();
                    if (token != null && DateTime.Now.Subtract(token.lastActivityDate.Value).Minutes < TokenValidMinitues)
                    {
                        token.ipAddress = "";
                        token.issueDate = DateTime.Now;
                        token.lastActivityDate = DateTime.Now;
                    }
                    else if (token != null && DateTime.Now.Subtract(token.lastActivityDate.Value).Minutes >= TokenValidMinitues)
                    {
                        token.authenticationTokenValue = Guid.NewGuid();
                        token.ipAddress = "";
                        token.issueDate = DateTime.Now;
                        token.lastActivityDate = DateTime.Now;
                    }
                    else
                    {
                        token = new authenticationtoken
                        {
                            authenticationTokenValue = Guid.NewGuid(),
                            ipAddress = "",
                            issueDate = DateTime.Now,
                            lastActivityDate = DateTime.Now,
                            user = user
                        };
                        vehmonEntities.authenticationtokens.Add(token);
                    }
                    response.GeneratedToken = token.authenticationTokenValue.ToString();
                    vehmonEntities.SaveChanges();
                }
            }
            return response;
        }

        public UserTokenValidationResponse ValidateToken(String userName, Guid token)
        {
            var response = new UserTokenValidationResponse();
            response.UserTokenState = UserTokenValidationState.Valid.ToString();
            using (var vehmonEntities = new vehmonEntities2())
            {
                var user = vehmonEntities.users.FirstOrDefault(x => x.username == userName);
                if (user == null)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                var userToken = user.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (userToken == null)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                if (DateTime.Now.Subtract(userToken.lastActivityDate.Value).Minutes > TokenValidMinitues)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
            }
            return response;
        }

        public UserTokenValidationResponse ValidateToken(Guid token)
        {
            var response = new UserTokenValidationResponse();
            response.UserTokenState = UserTokenValidationState.Valid.ToString();
            using (var vehmonEntities = new vehmonEntities2())
            {
                var userToken = vehmonEntities.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (userToken == null)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                if (DateTime.Now.Subtract(userToken.lastActivityDate.Value).Minutes > TokenValidMinitues)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
            }
            return response;
        }

        public UserTokenValidationResponse ValidateTokenInRole(Guid token, string role)
        {
            var response = new UserTokenValidationResponse();
            response.UserTokenState = UserTokenValidationState.Valid.ToString();
            using (var vehmonEntities = new vehmonEntities2())
            {
                var userToken = vehmonEntities.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (userToken == null)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                if (DateTime.Now.Subtract(userToken.lastActivityDate.Value).Minutes > TokenValidMinitues)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                var userRole = userToken.user.userrolemappings.FirstOrDefault(x => x.role.roleCode == role);
                if (userRole == null)
                {
                    response.UserTokenState = UserTokenValidationState.NotInRole.ToString();
                }
            }
            return response;
        }

        public UserTokenValidationResponse RenewToken(Guid token)
        {
            var response = new UserTokenValidationResponse();
            response.UserTokenState = UserTokenValidationState.Valid.ToString();
            using (var vehmonEntities = new vehmonEntities2())
            {
                var userToken = vehmonEntities.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (userToken == null)
                {
                    response.UserTokenState = UserTokenValidationState.Invalid.ToString();
                    return response;
                }
                userToken.lastActivityDate = DateTime.Now;
                vehmonEntities.SaveChanges();
            }
            return response;
        }

        public List<CompanyResponse> GetCompanies()
        {
            var companyResponses = new List<CompanyResponse>();
            using (var vehmonEntities = new vehmonEntities2())
            {
                companyResponses = vehmonEntities.companies.ToList().Select(x => new CompanyResponse
                {
                    CompanyId = x.companyID,
                    CompanyName = x.companyName
                }).ToList();
            }
            return companyResponses;
        }

        public int CreateCompany(string companyName)
        {
            using (var vehmonEntities = new vehmonEntities2())
            {
                var newCompanyEntity = new company
                {
                    companyName = companyName,
                    isActive = true,
                    dateAdded = DateTime.Now
                };
                vehmonEntities.companies.Add(newCompanyEntity);
                vehmonEntities.SaveChanges();
                return newCompanyEntity.companyID;
            }
        }

        public UserCreationResponse SetDeviceId(string token, string deviceId)
        {
            var response = new UserCreationResponse();
            var tokenGuid = Guid.Parse(token);
            response.IsSuccess = true;
            using (var context = new vehmonEntities2())
            {
                var userToken = context.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == tokenGuid);
                if (userToken == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Invalid Token";
                    return response;
                }
                userToken.user.deviceID = deviceId;
                context.SaveChanges();
            }
            return response;
        }
    }
}
