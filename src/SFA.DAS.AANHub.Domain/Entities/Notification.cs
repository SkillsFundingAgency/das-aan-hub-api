namespace SFA.DAS.AANHub.Domain.Entities;
public class Notification
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string TemplateName { get; set; } = null!;
    public string Tokens { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? SendAfterTime { get; set; }
    public DateTime? SentTime { get; set; }
    public bool IsSystem { get; set; }
    public string? ReferenceId { get; set; }
}
