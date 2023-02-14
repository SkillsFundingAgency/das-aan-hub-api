using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }

        public static implicit operator GetApprenticeMemberResult?(Apprentice apprentice)
        {
            if (apprentice == null)
                return null;

            return new GetApprenticeMemberResult
            {
                Email = apprentice.Email,
                Name = apprentice.Name,
                MemberId = apprentice.MemberId,
                Status = apprentice.Member.Status
            };
        }
    }
}
