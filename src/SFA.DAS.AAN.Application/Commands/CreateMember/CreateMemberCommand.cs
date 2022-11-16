
using MediatR;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Application.Commands.CreateMember
{
    public class CreateMemberCommand : IRequest<CreateMemberResponse>
    {
        public string id { get; set; }
        public MembershipUserTypes? UserType { get; set; }
        public DateTime joined { get; set; }
        public int? region { get; set; }
        public string? information { get; set; }
        public string? organisation { get; set; }
    }
}
