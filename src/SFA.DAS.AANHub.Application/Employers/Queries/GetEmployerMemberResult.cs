using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberResult
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Organisation { get; set; } = null!;
        public string Status { get; set; } = null!;


        public static implicit operator GetEmployerMemberResult?(Employer? employer)
        {
            if (employer == null)
                return null;

            return new GetEmployerMemberResult
            {
                Email = employer.Email,
                Name = employer.Name,
                Organisation = employer.Organisation,
                MemberId = employer.MemberId,
                Status = employer.Member.Status!
            };
        }
    }
}