using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Queries.GetApprentice
{
    public class GetApprenticeMemberResult
    {
        public Guid MemberId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public static implicit operator GetApprenticeMemberResult?(Apprentice apprentice)
        {
            if (apprentice == null)
                return null;

            return new GetApprenticeMemberResult
            {
                MemberId = apprentice.MemberId,
                Email = apprentice.Email,
                Name = apprentice.Name,
            };
        }
    }
}
