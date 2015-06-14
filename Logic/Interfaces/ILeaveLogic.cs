using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Contracts.Leave;
using Logic.Contracts.TimeManagement;

namespace Logic.Interfaces
{
    public interface ILeaveLogic
    {
        LeaveRequestResponse RequestLeave(Guid userToken, LeaveRequest leaveRequest);

        LeaveRequestResponse CancelLeave(Guid userToken, int leaveRequestId);

        LeaveRequestResponse GetAllLeaveRequests(Guid userToken);

        List<LeaveRequestContract> GetAllFutureLeaveRequests(Guid userToken);
    }
}
