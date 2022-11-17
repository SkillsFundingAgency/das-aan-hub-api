
using SFA.DAS.AAN.Application.Commands.CreateMember;


namespace SFA.DAS.AAN.Application.ApiResponses
{
    public class CreateMemberApiResponse
    {
        public Guid memberid { get; set; }
        public string usertype { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }

        public CreateMemberApiResponse(CreateMemberResponse result)
        {
            this.memberid = result.Member.Id;
            this.usertype = result.Member.UserType;
            this.status = result.Member.Status;
            this.created = result.Member.Created;
        }
    }
}
