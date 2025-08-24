using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Services.Core.Contracts
{
    public interface 
        IEmployeeService
    {
        Task<List<Employees>> GetAllAsync();
        Task<Employees?> GetByIdAsync(Guid id);
        Task<Employees?> GetByCardIdAsync(string cardId);
        
    }
}
