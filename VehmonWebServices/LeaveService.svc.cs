using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logic;
using Logic.Contracts.Leave;
using Logic.Interfaces;

namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LeaveService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LeaveService.svc or LeaveService.svc.cs at the Solution Explorer and start debugging.
   // [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class LeaveService : ILeaveServiceContract
    {
        private ILeaveLogic _leaveLogic;

        public LeaveService()
        {
            _leaveLogic = new LeaveLogic();    
        }

        public LeaveRequestResponse RequestLeave(string token, string startTime, string endTime, string leaveRequestType)
        {
           return _leaveLogic.RequestLeave(Guid.Parse(token),new LeaveRequest
            {
                EndTime = DateTime.ParseExact(endTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture),
                StartTime = DateTime.ParseExact(startTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture),
                LeaveRequestType = (LeaveRequestTypes)Enum.Parse(typeof(LeaveRequestTypes), leaveRequestType)
            });
        }

        public LeaveRequestResponse CancelLeave(string userToken, string leaveRequestId)
        {
            return _leaveLogic.CancelLeave(Guid.Parse(userToken),int.Parse(leaveRequestId));
        }

        public List<LeaveRequestContract> GetAllLeaveRequests(string userToken)
        {
            return _leaveLogic.GetAllLeaveRequests(Guid.Parse(userToken));
        }

        public List<LeaveRequestContract> GetAllFutureLeaveRequests(string userToken)
        {
            return _leaveLogic.GetAllFutureLeaveRequests(Guid.Parse(userToken));
        }

        public void DoWork()
        {
        }
    }
}
