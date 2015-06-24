using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Logic;
using Logic.Contracts.Leave;
using Logic.Contracts.Messaging;
using Logic.Contracts.TimeManagement;
using Logic.Contracts.UserCreation;
using Logic.Interfaces;
using RestSharp;

namespace TestServices
{
    class Program
    {
        private static IUserLogic _userLogic;
        private static ILeaveLogic _leaveLogic;
        private static ITimeTrackingLogic _timeTrackingLogic;
        private static IMessagingLogic _messagingLogic;
        private static Guid Token;
        public Program()
        {
            _userLogic = new UserLogic();
        }
        static void Main(string[] args)
        {
            //using (var context = new vehmonEntities2())
            //{
            //    var user = context.users.FirstOrDefault(x => x.username == "RenaldoB");
            //    var user2 = context.users.FirstOrDefault(x => x.username == "PhilipSc");
            //    user.company = user2.company;
            //    context.SaveChanges();
            //}
            var DatePast = DateTime.Now.AddHours(-7);
            var futureDate = DateTime.Now;
            var diffInMin = DatePast.Subtract(futureDate).TotalMinutes;

            string time = "2015-11-11-11-11";
            var dateTime = DateTime.ParseExact(time, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
            _userLogic = new UserLogic();
            _leaveLogic = new LeaveLogic();
            _timeTrackingLogic = new TimeTrackingLogic();
            _messagingLogic = new MessageLogic();

            Console.WriteLine("----------Main Menu------------");
            Console.WriteLine("Enter ManageCompanies to manage users");
            Console.WriteLine("Enter ManageUsers to manage users");
            Console.WriteLine("Enter ManageLeave to manage leave");
            Console.WriteLine("Enter ManageMessages to manage messages");
            Console.WriteLine("Enter ManageShifts to manage shifts");
            Console.WriteLine("Enter Exit to return to the previous menu");
            Console.WriteLine("--------------------------------");
            var command = Console.ReadLine();
            if (Token == null || Token == Guid.Empty)
                GetUserToken();

            switch (command)
            {
                case "ManageCompanies":
                    DoCompanyLogic();
                    break;
                case "ManageUsers":
                    DoUserLogic();
                    break;
                case "ManageLeave":
                    DoLeaveLogic();
                    break;
                case "ManageMessages":
                    DoMessageLogic();
                    break;
                case "ManageShifts":
                    DoShiftLogic();
                    break;
                case "Exit":
                    return;
                    break;
            }
            Main(new[] { "" });
        }

        private static void DoCompanyLogic()
        {
            Console.WriteLine("---------Company Menu------------");
            Console.WriteLine("Enter 1 to create a company");
            Console.WriteLine("Enter 2 to display all the companies");
            Console.WriteLine("--------------------------------");
            DoCompanyCommands();
        }

        private static void DoCompanyCommands()
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "1":
                    CreateCompany();
                    break;
                case "2":
                    DispayCompanies();
                    break;
            }
        }

        private static void DispayCompanies()
        {
            var allCompanies = _userLogic.GetCompanies();
            foreach (var companyResponse in allCompanies)
            {
                Console.WriteLine(String.Format("Company {0} with ID {1}",companyResponse.CompanyName , companyResponse.CompanyId));
            }
        }

        private static void CreateCompany()
        {
            Console.WriteLine("Please Enter the company Name");
            var companyName = Console.ReadLine();
            var companyID = _userLogic.CreateCompany(companyName);
            Console.WriteLine("Company created, the id is " + companyID);
        }

        private static void DoMessageLogic()
        {
            Console.WriteLine("---------Shift Menu------------");
            Console.WriteLine("Enter 1 to set the current user");
            Console.WriteLine("Enter 2 to display all the user's conversations");
            Console.WriteLine("Enter 3 to display all the user's unread messages");
            Console.WriteLine("Enter 4 to display all the user's unread messages for a conversation");
            Console.WriteLine("Enter 5 to remove a conversation for a user");
            Console.WriteLine("Enter 6 to send a message");
            Console.WriteLine("Enter 7 to get all users for a conversation");
            Console.WriteLine("Enter 8 to add a user to a conversation");
            Console.WriteLine("Enter 9 to create a conversation");
            Console.WriteLine("Enter 0 to display all messages for conversation");
            Console.WriteLine("Enter Exit to return to the previous menu");
            Console.WriteLine("--------------------------------");


            DoMessageCommands();
        }


        private static void DoMessageCommands()
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "1":
                    SetCurrentUser();
                    break;
                case "2":
                    ShowAllUsersConversations();
                    break;
                case "3":
                    ShowAllUnreadMessagesForUser();
                    break;
                case "4":
                    ShowConversationUnreadMessages();
                    break;
                case "5":
                    RemoveUserFromConversation();
                    break;
                case "6":
                    SendMessage();
                    break;
                case "7":
                    GetAllUsersForConversation();
                    break;
                case "8":
                    AddUserToConversation();
                    break;
                case "9":
                    CreateConversation();
                    break;
                case "0":
                    ShowConversationMessages();
                    break;
                case "Exit":
                    return;
                    break;
            }
            DoMessageCommands();
        }

        private static void RemoveUserFromConversation()
        {
            Console.WriteLine("Please enter the conversation id");
            var conId = int.Parse(Console.ReadLine());
            var response = _messagingLogic.RemoveConversation(Token, conId);
            Console.WriteLine("Remove cur user response " + response.RemoveStatus);


        }

        private static void AddUserToConversation()
        {
            Console.WriteLine("Please enter the conversation id");
            var conId = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter the username you want to add");
            var userName = Console.ReadLine();
            var response = _messagingLogic.AddUserToConversation(Token, conId, userName);
            Console.WriteLine("Add user response "+ response.MessageStatus);
        }

        private static void ShowAllUnreadMessagesForUser()
        {
            var result = _messagingLogic.GetAllUnreadMessages(Token);
            foreach (var messageResponse in result)
            {
                Console.WriteLine("Message from : " + messageResponse.Sender + " for conversation "+messageResponse.ConversationId);
                Console.WriteLine(messageResponse.Message);

            }
        }

        private static void GetAllUsersForConversation()
        {
            Console.WriteLine("Please enter the conversation id");
            var conId = int.Parse(Console.ReadLine());
            var response = _messagingLogic.GetAllUsersForConversation(Token, conId);
            foreach (var userDetailContract in response)
            {
                Console.WriteLine("User "+ userDetailContract.UserName+"  ");
            }
        }

        private static void ShowConversationMessages()
        {
            Console.WriteLine("Please enter the conversation id");
            var conId = int.Parse(Console.ReadLine());
            var result = _messagingLogic.GetAllMessagesForConversation(Token, conId);
            foreach (var messageResponse in result)
            {
                Console.WriteLine("Message from : " + messageResponse.Sender + " has been read:"+messageResponse.HasReceived);
                Console.WriteLine(messageResponse.Message);

            }
        }

        private static void ShowConversationUnreadMessages()
        {
            Console.WriteLine("Please enter the conversation id");
            var conId = int.Parse(Console.ReadLine());
            var result = _messagingLogic.GetAllUnreadMessagesForConversation(Token, conId);
            foreach (var messageResponse in result)
            {
                Console.WriteLine("Message from : "+messageResponse.Sender);
                Console.WriteLine(messageResponse.Message);

            }
        }

        private static void SendMessage()
        {
            Console.WriteLine("Please enter the conversation id you want to send a message to");
            var conId = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter the message you wish to send");
            var message = Console.ReadLine();
            var restult = _messagingLogic.SendMessage(Token, new SendMessageRequest
            {
                ConversationId = conId,
                Message = message,
                MessageSentDate = DateTime.Now
            });
            Console.WriteLine("Message send status is " + restult.MessageStatus);
        }

        private static void SetCurrentUser()
        {
            Console.WriteLine("Please enter the user name");
            var name = Console.ReadLine();
            Console.WriteLine("Please enter the user password");
            var password = Console.ReadLine();
            var restult = _userLogic.GetTokenForUser(name, password);
            if (restult.TokenGenerationState != TokenGenerationState.Valid)
            {
                Console.WriteLine("Invalid details");
                SetCurrentUser();
            }
            Token = Guid.Parse( restult.GeneratedToken);
            Console.WriteLine("User set to " + name + " token is " + Token);
        }

        private static void ShowAllUsersConversations()
        {
            var response = _messagingLogic.GetAllUserConversations(Token);
            foreach (var conversationResponse in response)
            {
                Console.WriteLine("Conversation with name " + conversationResponse.ConversationName + " and ID " + conversationResponse.ConversationId);
            }
        }

        private static void CreateConversation()
        {
            Console.WriteLine("Please enter conversation name");
            var name = Console.ReadLine();
            Console.WriteLine("Please enter a list of CSV usernames you wish to add, excluding your own");
            var names = Console.ReadLine();
            var restul = _messagingLogic.CreateConversation(Token, name, names);
            Console.WriteLine("Conversation creation status " + restul.CreateStatus);
        }

        private static void DoShiftLogic()
        {
            Console.WriteLine("---------Shift Menu------------");
            Console.WriteLine("Enter StartShift to start a shift");
            Console.WriteLine("Enter ShowAllShifts to display all current shifts");
            Console.WriteLine("Enter EndShift to end a shift");
            Console.WriteLine("Enter LogCoords to log coordinates");
            Console.WriteLine("Enter Exit to return to the previous menu");
            Console.WriteLine("--------------------------------");


            DoShiftCommands();
        }

        private static void DoShiftCommands()
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "StartShift":
                    StartShift();
                    break;
                case "ShowAllShifts":
                    DisplayAllShifts();
                    break;
                case "EndShift":
                    EndShift();
                    break;
                case "LogCoords":
                    LogCoords();
                    break;
                case "Exit":
                    return;
                    break;
            }
            DoShiftCommands();
        }

        private static void LogCoords()
        {
            Console.WriteLine("Please enter shift id to log coordinates to");
            var command = int.Parse(Console.ReadLine());
            var resutls = _timeTrackingLogic.LogCoordinatesToShift(Token, command, new Coordinate[]
            {
                new Coordinate
                {
                    Lattitude =(long) 33.33,
                    Longitude = (long)44.44,
                } ,new Coordinate
                {
                    Lattitude =(long) 44.33,
                    Longitude = (long)55.44,
                } ,
                new Coordinate
                {
                    Lattitude =(long) 55.33,
                    Longitude = (long)66.44,
                } 
            });
            Console.WriteLine("Status of coordinate log is " + resutls.LogState);
        }

        private static void DisplayAllShifts()
        {
            var results = _timeTrackingLogic.GetCurrentUserShifts(Token);
            results.ForEach(x => Console.WriteLine("Shift id " + x.ShiftId + " shift start time " + x.StartTime));
        }

        private static void EndShift()
        {
            Console.WriteLine("Please enter shift id to end");
            var command = int.Parse(Console.ReadLine());
            var resutl = _timeTrackingLogic.EndShift(Token, command, DateTime.Now);
            Console.WriteLine("Shift ending result is " + resutl.LogState);

        }

        private static void StartShift()
        {
            Console.WriteLine("Starting a shift");
            var resutl = _timeTrackingLogic.StartShift(new ShiftRequestContract
            {
                Lat = (long)33.333,
                Lng = (long)66.333,
                StartTime = DateTime.Now,
                UserToken = Token
            });
            Console.WriteLine("Shift result is " + resutl.LogState);

        }

        private static void DoLeaveLogic()
        {
            Console.WriteLine("---------Leave Menu------------");
            Console.WriteLine("Enter RequestLeave to request a leave");
            Console.WriteLine("Enter GetLeaves to display all valid leaves");
            Console.WriteLine("Enter GetAllLeaves to display all leaves including canceled ones");
            Console.WriteLine("Enter CancelLeave to cancel a leave");
            Console.WriteLine("Enter Exit to return to the previous menu");

            Console.WriteLine("--------------------------------");


            DoCommandsLeave();
        }

        private static void DoCommandsLeave()
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "RequestLeave":
                    RequestLeave();
                    break;
                case "GetLeaves":
                    GetAllLeaves();
                    break;
                case "GetAllLeaves":
                    GetAllLeavesAll();
                    break;
                case "CancelLeave":
                    CancelLeave();
                    break;
                case "Exit":
                    return;
                    break;
            }
            DoCommandsLeave();
        }

        private static void CancelLeave()
        {
            Console.WriteLine("Please enter leave id to cancel");
            var command = int.Parse(Console.ReadLine());
            var allLeaves = _leaveLogic.CancelLeave(Token, command);
            Console.WriteLine("Leave cancelation status " + allLeaves.RequestStatus);
        }

        private static void GetAllLeavesAll()
        {
            var allLeaves = _leaveLogic.GetAllFutureLeaveRequests(Token);
            foreach (var leaveRequestContract in allLeaves)
            {
                Console.WriteLine("Leave from " + leaveRequestContract.StartDateTime + " to " + leaveRequestContract.EndDateTime + " leave ID " + leaveRequestContract.LeaveRequestId + " of type " + leaveRequestContract.LeaveRequestType);
            }
        }

        private static void GetAllLeaves()
        {
            var allLeaves = _leaveLogic.GetAllLeaveRequests(Token);
            foreach (var leaveRequestContract in allLeaves.LeaveRequests)
            {
                Console.WriteLine("Leave from " + leaveRequestContract.StartDateTime + " to " + leaveRequestContract.EndDateTime + " leave ID " + leaveRequestContract.LeaveRequestId + " of type " + leaveRequestContract.LeaveRequestType);
            }
        }

        private static void RequestLeave()
        {
            Console.WriteLine("Requesting leave for 1 day");
            var restult = _leaveLogic.RequestLeave(Token, new LeaveRequest
            {
                StartTime = DateTime.Now.Add(TimeSpan.FromDays(1)),
                EndTime = DateTime.Now.Add(TimeSpan.FromDays(2)),
                LeaveRequestType = "Annual"
            });
            Console.WriteLine("Leave status " + restult.RequestStatus);

        }

        private static void DoUserLogic()
        {
            Console.WriteLine("---------User Menu------------");
            Console.WriteLine("Enter CreateUser to create user");
            Console.WriteLine("Enter GetToken to user token");
            Console.WriteLine("RenewToken to renew users token");
            Console.WriteLine("ValidateToken to validate token");
            Console.WriteLine("GetAllUsers to show a list of all users");
            Console.WriteLine("Enter Exit to return to the previous menu");
            Console.WriteLine("--------------------------------");


            DoCommandsUser();
        }

        private static void DoCommandsUser()
        {
            var command = Console.ReadLine();
            switch (command)
            {
                case "CreateUser":
                    CreateUser();
                    break;
                case "GetToken":
                    GetUserToken();
                    break;
                case "RenewToken":
                    RenewUserToken();
                    break;
                case "ValidateToken":
                    IsTokenValid();
                    break;
                case "GetAllUsers":
                    GetAllUsers();
                    break;
                case "Exit":
                    return;
                    break;
            }
            DoCommandsUser();
        }

        private static void CreateUser()
        {
            Console.WriteLine("Please type the user name");
            var userName = Console.ReadLine();
            CreateUser(userName);
        }

        private static void CreateUser(string userName)
        {
            var response = _userLogic.CreateUser(new UserDetailContract
            {
                CellPhoneNumber = "0745899420",
                DeviceId = "04564545454",
                EmailAddress = userName + "@vehmon.co.za",
                EmployerNumber = "654",
                FirstName = "Name for " + userName,
                IdNumber = "864864875454",
                LastName = "Surname",
                Password = "Password",
                Title = "Mr",
                UserName = userName,
            });
            if (response.IsSuccess)
                Console.WriteLine("User created");
            else
            {
                Console.WriteLine("User not created");
            }
        }

        private static void GetAllUsers()
        {
            var allUsers = _userLogic.GetAllUsers(Token);
            foreach (var userDetailContract in allUsers)
            {
                Console.WriteLine(userDetailContract.UserName);
            }
        }

        private static void IsTokenValid()
        {
            var tokenvalid = _userLogic.ValidateToken("PhilipSc", Token);
            Console.WriteLine(tokenvalid.UserTokenState);

        }


        private static void GetUserToken()
        {
            var token = _userLogic.GetTokenForUser("PhilipSc", "Password");

            if (token.TokenGenerationState != TokenGenerationState.Valid)
            {
                CreateUser("PhilipSc");
            }
            else
            {
                token = _userLogic.GetTokenForUser("PhilipSc", "Password");
            }

            Console.WriteLine("The token is " + token.GeneratedToken);
            Token = Guid.Parse(token.GeneratedToken);
        }

        private static void RenewUserToken()
        {
            var token = _userLogic.RenewToken(Token);
            Console.WriteLine("Renewed tthe token " + token.UserTokenState.ToString());
        }

    }
}
