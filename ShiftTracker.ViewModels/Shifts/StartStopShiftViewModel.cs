using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Shifts
{
    public class StartStopShiftViewModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public Shift? CurrentShift { get; set; }
    }

}
