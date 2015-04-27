using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TimeLogging" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TimeLogging.svc or TimeLogging.svc.cs at the Solution Explorer and start debugging.
    public class TimeLogging : ITimeLogging
    {
        public void DoWork()
        {
        }

        public ShiftResponse StartShift(string userToken, decimal lat, decimal lng)
        {
            throw new NotImplementedException();
        }

        public ShiftResponse StopShift(string userToken, decimal lat, decimal lng)
        {
            throw new NotImplementedException();
        }

        public ShiftResponse AddShift(string userToken, decimal startLat, decimal endLng, decimal endLat, DateTime startTime,
            DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public ShiftResponse EditShift(string userToken, int shiftId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public List<Shift> GetAllShiftsForUser(string userToken)
        {
            throw new NotImplementedException();
        }

        public Shift GetShift(string userToken, int shiftId)
        {
            throw new NotImplementedException();
        }
    }
}
