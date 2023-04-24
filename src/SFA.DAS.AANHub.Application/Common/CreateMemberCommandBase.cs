namespace SFA.DAS.AANHub.Application.Common;

public abstract class CreateMemberCommandBase
{
    protected CreateMemberCommandBase() => Id = Guid.NewGuid();
    internal Guid Id { get; init; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Joined { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; }
}
