namespace ShiftTracker.Data.Models
{
    public class Break
    {
        public int Id { get; set; }

        public Shift Shift { get; set; } = null!;
        public int ShiftId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }

}
