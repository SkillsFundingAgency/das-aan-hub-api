using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Partners.Queries
{
    public class GetPartnerMemberResult
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Organisation { get; set; } = null!;

        public static implicit operator GetPartnerMemberResult?(Partner? partner)
        {
            if (partner == null)
                return null;

            return new GetPartnerMemberResult
            {
                Email = partner.Email,
                Organisation = partner.Organisation,
                Name = partner.Name,
                MemberId = partner.MemberId,
            };
        }
    }
}
