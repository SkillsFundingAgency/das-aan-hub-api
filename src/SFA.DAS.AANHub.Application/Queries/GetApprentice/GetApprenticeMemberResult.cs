using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Queries.GetApprentice
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public static implicit operator GetApprenticeMemberResult(SFA.DAS.AANHub.Domain.Entities.Apprentice apprentice) =>
            new GetApprenticeMemberResult
            {
                MemberId = apprentice.MemberId,
                Email = apprentice.Email,
                Name = apprentice.Name,
            };
    }
}
