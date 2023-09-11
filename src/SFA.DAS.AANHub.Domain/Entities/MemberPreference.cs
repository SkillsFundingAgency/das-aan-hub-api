namespace SFA.DAS.AANHub.Domain.Entities;
public class MemberPreference
{
    public int Id { get; set; }
    public Guid MemberId { get; set; }
    public int PreferenceId { get; set; }
    public bool AllowSharing { get; set; }
}
