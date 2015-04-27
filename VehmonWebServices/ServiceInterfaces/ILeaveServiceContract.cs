using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Logic;
using Logic.Contracts.Leave;
using Logic.Contracts.TimeManagement;
using Logic.Contracts.UserCreation;

namespace VehmonWebServices
{
    [ServiceContract]
    public interface ILeaveServiceContract
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "RequestLeave/{token}/{startTime}/{endTime}/{leaveRequestType}", ResponseFormat = WebMessageFormat.Json)]
        LeaveRequestResponse RequestLeave(string token, string startTime, string endTime, string leaveRequestType);

        [OperationContract]
        [WebInvoke(UriTemplate = "CancelLeave/{userToken}/{leaveRequestId}", ResponseFormat = WebMessageFormat.Json)]
        LeaveRequestResponse CancelLeave(string userToken, String leaveRequestId);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllLeaveRequests/{userToken}", ResponseFormat = WebMessageFormat.Json)]
        List<LeaveRequestContract> GetAllLeaveRequests(string userToken);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllFutureLeaveRequests/{userToken}", ResponseFormat = WebMessageFormat.Json)]
        List<LeaveRequestContract> GetAllFutureLeaveRequests(string userToken);

        [OperationContract]
        void DoWork();
    }
}
