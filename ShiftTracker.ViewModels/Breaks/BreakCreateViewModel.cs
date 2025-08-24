using ShiftTracker.ViewModels.Shifts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Breaks
{
    public class BreakCreateViewModel
    {
        [Required] 
        public int ShiftId { get; set; }

        [Required] 
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public IEnumerable<ShiftViewModel> Shifts { get; set; } = new List<ShiftViewModel>();

    }
}
