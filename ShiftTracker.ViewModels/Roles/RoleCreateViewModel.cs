using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.ViewModels.Roles
{
    public class RoleCreateViewModel
    {
        [Required] 
        public string Name { get; set; } = null!;
    }
}
