
namespace SFA.DAS.AANHub.Domain.Entities
{
    public class Calendar
    {
        public long Id { get; set; }
        public string? CalendarName { get; set; }
        public string? Description { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }
}
