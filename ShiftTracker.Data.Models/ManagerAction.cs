using ShiftTracker.Data.Models;

public class ManagerAction
{
    public int Id { get; set; }

    public Employees Manager { get; set; } = null!;
    public Guid ManagerId { get; set; }

    public string Action { get; set; } = null!;

    public Guid? EmployeeId { get; set; }      // FK for affected employee
    public Employees? Employee { get; set; }   // Navigation property

    public DateTime Timestamp { get; set; }
}