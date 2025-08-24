using ShiftTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Employees
{
    public class EmployeeCreateViewModel
    {
        [Required] 
        public Guid Id { get; set; } = new Guid();

        [Required] 
        public string Name { get; set; } = null!;

        [Required] 
        public string CardId { get; set; } = null!;

        [Required] 
        public string Pin { get; set; } = null!;

        [Required] 
        public int PositionId { get; set; }

        [Required] 
        public int RoleId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public IEnumerable<Position> Positions { get; set; } 
            = new HashSet<Position>();

        public IEnumerable<Role> Roles { get; set; } 
            = new HashSet<Role>();

    }
}
