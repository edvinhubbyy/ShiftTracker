using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftTracker.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Employees> Employees { get; set; } 
            = new HashSet<Employees>();

    }
}
