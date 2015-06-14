using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Logic;
using Logic.Contracts.TimeManagement;
using Logic.Contracts.UserCreation;

namespace VehmonWebServices
{
    [ServiceContract]
    public interface ITimeTrackingServiceContract
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "StartShift/{token}/{clockInLat}/{clockOutLat}/{startTime}", ResponseFormat = WebMessageFormat.Json)]
        ShiftResponse StartShift(string token, string clockInLat, string clockOutLat, string startTime);

        [OperationContract]
        [WebInvoke(UriTemplate = "EndShift/{userToken}/{shiftId}/{endDate}", ResponseFormat = WebMessageFormat.Json)]
        ShiftResponse EndShift(string userToken, string shiftId, string endDate);

        [OperationContract]
        [WebInvoke(UriTemplate = "LogCoordinatesToShift/{token}/{shiftId}/{csvCoords}", ResponseFormat = WebMessageFormat.Json)]
        ShiftResponse LogCoordinatesToShift(string token, String shiftId, string csvCoords);

        [OperationContract]
        [WebGet(UriTemplate = "GetCurrentUserShifts/{token}", ResponseFormat = WebMessageFormat.Json)]
        List<ShiftContract> GetCurrentUserShifts(string token);

        [OperationContract]
        void DoWork();
    }
}
