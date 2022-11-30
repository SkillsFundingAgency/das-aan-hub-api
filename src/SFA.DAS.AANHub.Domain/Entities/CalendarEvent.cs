
namespace SFA.DAS.AANHub.Domain.Entities
{
    public class CalendarEvent : EntityBase
    {
        public Guid Id { get; set; }
        public Int64 CalendarId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Description { get; set; }
        public Int32 RegionId { get; set; }
        public string? Location { get; set; }
        public string? Postcode { get; set; }
        public string? EventLink { get; set; }
        public string? Contact { get; set; }
        public string? ContactEmail { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public DateTime? Deleted { get; set; }
        public bool IsActive { get; set; }
    }
}
