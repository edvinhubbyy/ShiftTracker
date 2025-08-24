using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Services.Core.Contracts
{
    public interface IShiftService
    {
        Task<List<Shift>> GetShiftsForEmployeeAsync(Guid employeeId);
        Task<Shift> StartShiftAsync(Guid employeeId);
        Task EndShiftAsync(int shiftId);
        Task<Break> StartBreakAsync(int shiftId);
        Task EndBreakAsync(int breakId);
    }
}
