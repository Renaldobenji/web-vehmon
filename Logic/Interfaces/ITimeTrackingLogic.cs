using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Contracts.TimeManagement;

namespace Logic.Interfaces
{
    public interface ITimeTrackingLogic
    {
        ShiftResponse StartShift(ShiftRequestContract shiftRequest);

        ShiftResponse EndShift(Guid userToken, int shiftId);

        ShiftResponse LogCoordinatesToShift(Guid userToken, int shiftId, Coordinate[] coordinates);


        List<ShiftContract> GetCurrentUserShifts(Guid token);
    }
}
