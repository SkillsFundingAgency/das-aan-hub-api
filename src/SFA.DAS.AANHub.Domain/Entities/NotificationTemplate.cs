namespace SFA.DAS.AANHub.Domain.Entities;
public class NotificationTemplate
{
    public long Id { get; set; }
    public string Description { get; set;} = null!;
    public string TemplateName { get; set; } = null!;
    public bool IsActive { get; set; }

}
