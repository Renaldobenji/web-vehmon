using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Logic.Contracts.TimeManagement;
using Logic.Contracts.UserCreation;
using Logic.Interfaces;

namespace Logic
{//todo : add getcurrentshift

    public class TimeTrackingLogic : ITimeTrackingLogic
    {
        private IUserLogic _userLogic;

        public TimeTrackingLogic()
        {
            _userLogic = new UserLogic();
        }

        public ShiftResponse StartShift(ShiftRequestContract shiftRequest)
        {
            var response = new ShiftResponse();
            response.LogState = ShiftLogState.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == shiftRequest.UserToken));
                if (userCurrent == null)
                {
                    response.LogState = ShiftLogState.InvalidToken;
                    return response;
                }                
                var newShift = new timetracking
                {
                    clockInLat = shiftRequest.Lat,
                    clockInLng = shiftRequest.Lat,
                    clockInTime = shiftRequest.StartTime
                };
                userCurrent.timetrackings.Add(newShift);
                context.SaveChanges();
                response.ShiftId = newShift.timeTrackingID;
            }
            return response;
        }

        public ShiftResponse EndShift(Guid userToken, int shiftId)
        {
            var response = new ShiftResponse();
            response.LogState = ShiftLogState.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    response.LogState = ShiftLogState.InvalidToken;
                    return response;
                }
                var currentShift = userCurrent.timetrackings.FirstOrDefault(x => x.timeTrackingID == shiftId);                
                currentShift.clockOutTime = DateTime.Now;
                context.SaveChanges();
            }
            return response;
        }

        public ShiftResponse LogCoordinatesToShift(Guid userToken, int shiftId, Coordinate[] coordinates)
        {
            var response = new ShiftResponse();
            response.LogState = ShiftLogState.Success;
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == userToken));
                if (userCurrent == null)
                {
                    response.LogState = ShiftLogState.InvalidToken;
                    return response;
                }
                var currentShift = userCurrent.timetrackings.FirstOrDefault(x => x.timeTrackingID == shiftId);
                
                if (currentShift.routes.Count < 0)
                {
                    currentShift.routes.Add(new route
                    {
                        startTime = DateTime.Now
                    });
                    context.SaveChanges();
                }
                var route = currentShift.routes.FirstOrDefault();
                if (route == null)
                {
                    route = new route
                    {
                        startTime = DateTime.Now,
                    };
                    currentShift.routes.Add(route);
                    context.SaveChanges();
                }
                foreach (var coord in coordinates)
                {
                    if (route != null)
                        route.coords.Add(new coord
                        {
                            lat = coord.Lattitude,
                            lng = coord.Longitude,
                            time = coord.Date
                        });
                }
                context.SaveChanges();
            }
            return response;
        }

        public List<ShiftContract> GetCurrentUserShifts(Guid token)
        {
            List<ShiftContract> respoContracts = new List<ShiftContract>();
            using (var context = new vehmonEntities2())
            {
                var userCurrent =
                    context.users.FirstOrDefault(
                        x => x.authenticationtokens.Any(a => a.authenticationTokenValue == token));
                if (userCurrent == null)
                {

                    return respoContracts;
                }
                return userCurrent.timetrackings.Where(x => x.clockOutTime == null).ToList().Select(x => new ShiftContract
                    {
                        StartTime = x.clockInTime,
                        ShiftId = x.timeTrackingID
                    }).ToList();
                    
            }
        }
    }
}
