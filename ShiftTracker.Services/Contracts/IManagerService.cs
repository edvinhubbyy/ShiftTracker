using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Services.Core.Contracts
{
    public interface IManagerService
    {
        Task RecordActionAsync(Guid managerId, string action, Guid? employeeId = null);
        Task<List<Employees>> GetAllAsync();
        Task AdjustShiftAsync(Guid employeeId, DateTime? checkInTime, DateTime? checkOutTime);
        Task<Employees?> GetByIdAsync(Guid id);
        Task CreateAsync(Employees employee);
        Task UpdateAsync(Employees employee);
        Task DeleteAsync(Guid id);

        Task ResetPinAsync(Guid employeeId, string newPin);
    }
}
