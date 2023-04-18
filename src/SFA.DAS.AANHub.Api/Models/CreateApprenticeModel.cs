using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreateApprenticeModel
    {
        public Guid ApprenticeId { get; set; }
        public DateTime Joined { get; set; }
        public int? RegionId { get; set; }
        public string? Information { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator CreateApprenticeMemberCommand(CreateApprenticeModel model) => new()
        {
            ApprenticeId = model.ApprenticeId,
            Joined = model.Joined,
            Information = model.Information,
            Email = model.Email,
            Name = model.Name,
            RegionId = model.RegionId
        };
    }
}