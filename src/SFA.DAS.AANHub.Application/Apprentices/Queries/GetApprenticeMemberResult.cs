using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = null!;

        public static implicit operator GetApprenticeMemberResult?(Apprentice apprentice)
        {
            if (apprentice == null)
                return null;

            return new GetApprenticeMemberResult
            {
                Email = apprentice.Email,
                Name = apprentice.Name,
                MemberId = apprentice.MemberId,
                Status = apprentice.Member.Status!
            };
        }
    }
}
