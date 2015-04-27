using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.SqlServer.Server;

namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITimeLogging" in both code and config file together.
    [ServiceContract]
    public interface ITimeLogging
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebInvoke(UriTemplate = "StartShift/{userToken}/{lat}/{lng}")]
        ShiftResponse StartShift(string userToken, decimal lat, decimal lng);

        [OperationContract]
        [WebInvoke(UriTemplate = "StopShift/{userToken}/{lat}/{lng}")]
        ShiftResponse StopShift(string userToken, decimal lat, decimal lng);

        [OperationContract]
        [WebInvoke(UriTemplate = "AddShift/{userToken}/{startLat}/{endLng}/{endLat}/{startTime}/{endTime}")]
        ShiftResponse AddShift(string userToken, decimal startLat, decimal endLng, decimal endLat, DateTime startTime, DateTime endTime);

        [OperationContract]
        [WebInvoke(UriTemplate = "EditShift/{userToken}/{shiftId}/{startTime}/{endTime}")]
        ShiftResponse EditShift(string userToken, int shiftId, DateTime startTime, DateTime endTime);

        [OperationContract]
        [WebGet(UriTemplate = "GetAllShiftsForUser/{userToken}")]
        List<Shift> GetAllShiftsForUser(string userToken);

        [OperationContract]
        [WebGet(UriTemplate = "GetShift/{userToken}/{shiftId}")]
        Shift GetShift(string userToken, int shiftId);
    }

    [DataContract]
    public class Shift
    {
        public string UserName { get; set; }

        public decimal StartLat { get; set; }

        public decimal StartLng { get; set; }

        public decimal EndLat { get; set; }

        public decimal EndLng { get; set; }

        public decimal StartTime { get; set; }

        public decimal EndTime { get; set; }
    }

    [DataContract]
    public class ShiftResponse  
    {
    }
}
