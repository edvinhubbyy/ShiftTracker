using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Employees
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string CardId { get; set; } = null!;
        public string Pin { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
