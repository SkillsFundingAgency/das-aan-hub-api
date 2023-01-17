
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.AANHub.Application.Commands.CreateMember;

namespace SFA.DAS.AANHub.Application.Responses
{
    [ExcludeFromCodeCoverage]
    public class CreateMemberApiResponse
    {
        public Guid MemberId { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public DateTime Created { get; set; }

        public CreateMemberApiResponse(CreateMemberResponse result)
        {
            if (result.Member == null)
            {
                throw new ArgumentNullException(nameof(result), "Member is null");
            }

            MemberId = result.Member.Id;
            UserType = result.Member.UserType?.ToString();
            Status = result.Member.Status?.ToString();
            Created = result.Member.Created;
        }
    }
}
