using ShiftTracker.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.ManagerActions
{
    public class ManagerActionCreateViewModel
    {
        [Required] public Guid ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
        [Required] public string Action { get; set; } = null!;
        public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
