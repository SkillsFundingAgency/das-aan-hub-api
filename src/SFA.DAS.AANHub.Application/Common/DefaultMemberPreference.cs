namespace SFA.DAS.AANHub.Application.Common;

public class DefaultMemberPreference
{
    public DefaultMemberPreference(int id, bool allowSharing)
    {
        Id = id;
        AllowSharing = allowSharing;
    }
    public int Id { get; set; }
    public bool AllowSharing { get; set; }
}