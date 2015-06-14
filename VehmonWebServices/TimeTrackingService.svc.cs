using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logic;
using Logic.Contracts.TimeManagement;
using Logic.Interfaces;

namespace VehmonWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TimeTrackingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TimeTrackingService.svc or TimeTrackingService.svc.cs at the Solution Explorer and start debugging.
   // [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TimeTrackingService : ITimeTrackingServiceContract
    {
        private ITimeTrackingLogic _timeTrackingLogic;

        public TimeTrackingService()
        {
            _timeTrackingLogic = new TimeTrackingLogic();            
        }

        public List<ShiftContract> GetCurrentUserShifts(string token)
        {
            return _timeTrackingLogic.GetCurrentUserShifts(Guid.Parse(token));
        }

        public void DoWork()
        {
        }

        public ShiftResponse StartShift(string token, string clockInLat, string clockOutLat, string startTime)
        {
            var datetime = DateTime.ParseExact(startTime.ToString(CultureInfo.InvariantCulture), "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
           return  _timeTrackingLogic.StartShift(new ShiftRequestContract
            {
                UserToken = Guid.Parse(token),
                Lat = decimal.Parse( clockInLat),
                Lng = decimal.Parse(clockOutLat),
                StartTime = datetime,
            });
        }

        public Logic.Contracts.TimeManagement.ShiftResponse EndShift(string userToken, string shiftId, string endTime)
        {
            var datetime = DateTime.ParseExact(endTime.ToString(CultureInfo.InvariantCulture), "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture);
            return _timeTrackingLogic.EndShift(Guid.Parse(userToken), int.Parse(shiftId), datetime);
        }

        public Logic.Contracts.TimeManagement.ShiftResponse LogCoordinatesToShift(string userToken, string shiftId, string coords)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (var i = 0;i< coords.Count(f=>f==',');i=i+3)
            {
                var splitString = coords.Split(","[0]);
                var newCoord = new Coordinate
                {
                    Lattitude = decimal.Parse(splitString[i]),
                    Longitude = decimal.Parse(splitString[i + 1]),
                    Date = DateTime.ParseExact(splitString[i + 2], "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture)
                };
                coordinates.Add(newCoord);
            }
            return _timeTrackingLogic.LogCoordinatesToShift(Guid.Parse(userToken), int.Parse(shiftId), coordinates.ToArray());
        }
    }
}
