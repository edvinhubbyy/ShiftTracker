using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.ManagerActions
{
    public class ManagerActionViewModel
    {
        public int Id { get; set; }
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; } = null!;
        public Guid? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string Action { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
