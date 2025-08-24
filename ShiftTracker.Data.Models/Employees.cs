namespace ShiftTracker.Data.Models
{
    public class Employees
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Position Position { get; set; } = null!;

        public int PositionId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public Role Role { get; set; } = null!;

        public int RoleId { get; set; }

        public string CardId { get;set; } = null!;

        public string Pin { get; set; }

        public bool IsDeleted { get; set; }


        public ICollection<Shift> Shifts { get; set; }
            = new HashSet<Shift>();
        
        public ICollection<AuditLog> AuditLogs { get; set; }
            = new HashSet<AuditLog>();

        public ICollection<ManagerAction> ManagerActions { get; set; }
            = new HashSet<ManagerAction>();


    }
}
