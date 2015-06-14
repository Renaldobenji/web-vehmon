using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Logic.Contracts.Leave;
using Logic.Interfaces;

namespace Logic
{
    public class LeaveLogic : ILeaveLogic
    {
        public LeaveRequestResponse RequestLeave(Guid userToken, LeaveRequest leaveRequest)
        {
            var response = new LeaveRequestResponse();
            response.RequestStatus = LeaveRequestStatus.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    response.RequestStatus = LeaveRequestStatus.InvalidToken;
                    return response;
                }
                var leaveType = context.absencetypes.FirstOrDefault(x => x.absenceTypeCode == leaveRequest.LeaveRequestType.ToString());
                var newLeave = new userabsence
                {
                    absencetype = leaveType,
                    fromDate = leaveRequest.StartTime,
                    toDate = leaveRequest.EndTime,
                };
                userCurrent.userabsences.Add(newLeave);
                context.SaveChanges();
                response.RequestId = newLeave.userAbsenseID;
            }
            return response;
        }

        public LeaveRequestResponse CancelLeave(Guid userToken, int leaveRequestId)
        {
            var response = new LeaveRequestResponse();
            response.RequestStatus = LeaveRequestStatus.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    response.RequestStatus = LeaveRequestStatus.InvalidToken;
                    return response;
                }
                var leave = context.userabsences.FirstOrDefault(x => x.userId == leaveRequestId);
                if (leave == null)
                {
                    response.RequestStatus = LeaveRequestStatus.LeaveNotFound;
                    return response;
                }
                var leaveType = context.absencetypes.FirstOrDefault(x => x.absenceTypeCode == LeaveRequestTypes.Canceled.ToString());
                leave.absencetype = leaveType;
                context.SaveChanges();
            }
            return response;
        }

        public LeaveRequestResponse GetAllLeaveRequests(Guid userToken)
        {
            LeaveRequestResponse response = new LeaveRequestResponse();            
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    return response;
                }

                var allRequests = userCurrent.userabsences.Where(x => x.absencetype.absenceTypeCode != LeaveRequestTypes.Canceled.ToString()).OrderByDescending(x => x.fromDate).Take(20).ToList();

                response.LeaveRequests = allRequests.Select(x => new LeaveRequestContract
                {
                    EndDateTime = x.toDate.ToString("yyyy-MM-dd"),
                    StartDateTime = x.fromDate.ToString("yyyy-MM-dd"),
                    LeaveRequestId = x.userAbsenseID,
                    LeaveRequestType = ((LeaveRequestTypes)x.absencetype.absenceTypeID).ToString(),
                    Status = (x.approved.HasValue ? (x.approved.Value == false ? "Declined" : "Approved") : "Pending")
                }).ToList();

                response.AvailableBalance = new Random().Next(30);//This must be replaced
            }

            return response;
        }

        public List<LeaveRequestContract> GetAllFutureLeaveRequests(Guid userToken)
        {
            var response = new List<LeaveRequestContract>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent = context.users.FirstOrDefault(x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    return response;
                }
                var allRequests = userCurrent.userabsences.OrderByDescending(x => x.fromDate).Take(10).ToList();
                return allRequests.Select(x => new LeaveRequestContract
                {
                    EndDateTime = x.toDate.ToShortDateString(),
                    StartDateTime = x.fromDate.ToShortDateString(),
                    LeaveRequestId = x.absenseTypeID,
                    LeaveRequestType = ((LeaveRequestTypes)x.absencetype.absenceTypeID).ToString()
                }).ToList();
            }
        }
    }
}
