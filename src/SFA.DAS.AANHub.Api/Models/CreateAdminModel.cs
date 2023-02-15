using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreateAdminModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Information { get; set; } = null!;
        public DateTime Joined { get; set; }
        public List<int>? Regions { get; set; }

        public static implicit operator CreateAdminMemberCommand(CreateAdminModel model) => new()
        {
            Email = model.Email,
            Name = model.Name,
            Joined = model.Joined,
            UserName = model.UserName,
            Information = model.Information,
            Regions = model.Regions
        };
    }
}