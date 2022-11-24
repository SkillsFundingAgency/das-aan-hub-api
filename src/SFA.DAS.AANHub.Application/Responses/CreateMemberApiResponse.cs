
using SFA.DAS.AANHub.Application.Commands.CreateMember;

namespace SFA.DAS.AANHub.Application.Responses
{
    public class CreateMemberApiResponse
    {
        public Guid MemberId { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public DateTime Created { get; set; }

        public CreateMemberApiResponse(CreateMemberResponse result)
        {
            MemberId = result.Member.Id;
            UserType = result.Member.UserType;
            Status = result.Member.Status;
            Created = result.Member.Created;
        }
    }
}
