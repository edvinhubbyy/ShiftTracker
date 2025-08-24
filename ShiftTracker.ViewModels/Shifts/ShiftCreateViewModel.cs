using ShiftTracker.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Shifts
{
    public class ShiftCreateViewModel
    {
        [Required] 
        public Guid EmployeeId { get; set; }

        [Required] 
        public DateTime CheckInTime { get; set; } = DateTime.Now;

        public DateTime? CheckOutTime { get; set; }

        public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
