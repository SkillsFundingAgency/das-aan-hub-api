using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Common;

public class GetMemberResult
{
    public Guid MemberId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }

    public static implicit operator GetMemberResult?(Member? member)
    {
        if (member == null)
            return null;

        return new GetMemberResult
        {
            MemberId = member.Id,
            Email = member.Email,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Status = member.Status!,
            OrganisationName = member.OrganisationName,
            RegionId = member.RegionId
        };
    }
}
