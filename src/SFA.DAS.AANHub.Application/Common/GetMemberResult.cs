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
    public string UserType { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool? IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;

    public EmployerModel? Employer { get; set; } = null!;
    public ApprenticeModel? Apprentice { get; set; } = null!;

    public static implicit operator GetMemberResult?(Member? member)
    {
        if (member == null)
            return null;

        return new GetMemberResult
        {
            MemberId = member.Id,
            UserType = member.UserType,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            Status = member.Status!,
            JoinedDate = member.JoinedDate,
            EndDate = member.EndDate,
            RegionId = member.RegionId,
            OrganisationName = member.OrganisationName,
            LastUpdatedDate = member.LastUpdatedDate,
            IsRegionalChair = member.IsRegionalChair,
            FullName = member.FullName,
            Apprentice = new ApprenticeModel(member?.Apprentice?.ApprenticeId),
            Employer = new EmployerModel(member?.Employer?.AccountId, member?.Employer?.UserRef)
        };
    }
}

public record EmployerModel(long? AccountId, Guid? UserRef);

public record ApprenticeModel(Guid? ApprenticeId);
