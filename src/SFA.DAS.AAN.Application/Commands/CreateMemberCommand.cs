
using MediatR;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Application.Commands.CreateMember
{
    public class CreateMemberCommand : IRequest<CreateMemberResponse>
    {
        public string id { get; set; }
        public string UserType { get; set; }
        public DateTime joined { get; set; }
        public int? region { get; set; }
        public string information { get; set; }
        public string organisation { get; set; }

        public CreateMemberCommand(string id, string UserType, DateTime joined, int? region, string information, string organisation)
        {
            this.id = id;
            this.UserType = UserType;
            this.joined = joined;
            this.region = region;
            this.information = information;
            this.organisation = organisation;
        }
    }
}
