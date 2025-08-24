using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Data.Models
{
    public class Shift
    {
        public int Id { get; set; }

        public Employees Employee { get; set; } = null!;
        public Guid EmployeeId { get; set; }

        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }


        public ICollection<Break> Breaks { get; set; } 
            = new HashSet<Break>();

        public ICollection<AuditLog> AuditLogs { get; set; } 
            = new HashSet<AuditLog>();

    }
}
