using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public Employees Employee { get; set; } = null!;
        public Guid EmployeeId { get; set; }

        public Shift? Shift { get; set; }
        public int? ShiftId { get; set; }

        public string EventType { get; set; } = null!;

        public DateTime Timestamp { get; set; }
    }
}
