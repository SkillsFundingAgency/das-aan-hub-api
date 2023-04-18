using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreatePartnerModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime Joined { get; set; }
        public int? RegionId { get; set; }
        public string? Information { get; set; }
        public string Organisation { get; set; } = null!;

        public static implicit operator CreatePartnerMemberCommand(CreatePartnerModel model) => new()
        {
            Organisation = model.Organisation,
            Email = model.Email,
            Name = model.Name,
            Information = model.Information,
            Joined = model.Joined,
            RegionId = model.RegionId,
            UserName = model.UserName
        };
    }
}