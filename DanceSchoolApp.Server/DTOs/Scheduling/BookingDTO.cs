namespace DanceSchoolApp.Server.DTOs.Scheduling
{
    public class DaySlotResponse
    {
        public DateOnly Date { get; set; }
        public List<CoachSlot> Slots { get; set; } = new();
    }

    public class CoachSlot
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<int> ModalityIds { get; set; } = new();
        public List<string> ModalityNames { get; set; } = new();
    }
}
