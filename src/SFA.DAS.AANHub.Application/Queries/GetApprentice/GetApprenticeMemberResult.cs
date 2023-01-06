using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Queries.GetApprentice
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public long ApprenticeId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator GetApprenticeMemberResult(SFA.DAS.AANHub.Domain.Entities.Apprentice apprentice) =>
            new GetApprenticeMemberResult
            {
                MemberId = apprentice.MemberId,
                ApprenticeId = apprentice.ApprenticeId,
                Email = apprentice.Email,
                Name = apprentice.Name,
                LastUpdated = apprentice.LastUpdated,
                IsActive = apprentice.IsActive,
            };
    }
}
