namespace SFA.DAS.AANHub.Application.Common;

public abstract class CreateMemberCommandBase
{
    protected CreateMemberCommandBase() => MemberId = Guid.NewGuid();
    internal Guid MemberId { get; init; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? JoinedDate { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; }
    public List<ProfileValue> ProfileValues { get; set; } = new();
}
