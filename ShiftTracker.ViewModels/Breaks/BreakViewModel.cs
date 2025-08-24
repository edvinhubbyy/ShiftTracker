using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Breaks
{
    public class BreakViewModel
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
