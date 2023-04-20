using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Status { get; set; } = null!;

        public static implicit operator GetApprenticeMemberResult?(Apprentice apprentice)
        {
            if (apprentice == null)
                return null;

            return new GetApprenticeMemberResult
            {
                Email = apprentice.Member.Email,
                FirstName = apprentice.Member.FirstName,
                LastName = apprentice.Member.LastName,
                MemberId = apprentice.MemberId,
                Status = apprentice.Member.Status!
            };
        }
    }
}
