using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Employees
{
    public class EmployeeShiftViewModel
    {
        public Guid EmployeeId { get; set; }         
        public string EmployeeName { get; set; } = "Unknown";

        public Shift? CurrentShift { get; set; }

        public List<Shift> ShiftHistory { get; set; } = new List<Shift>();
    }


}
