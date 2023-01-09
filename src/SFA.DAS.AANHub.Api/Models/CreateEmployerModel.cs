using SFA.DAS.AANHub.Application.Employers.Commands;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreateEmployerModel
    {
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public string? Organisation { get; set; }
        public DateTime Joined { get; set; }
        public List<int>? Regions { get; set; }
        public string? Information { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator CreateEmployerMemberCommand(CreateEmployerModel model) => new()
        {
            AccountId = model.AccountId,
            UserId = model.UserId,
            Organisation = model.Organisation,
            Email = model.Email,
            Name = model.Name,
            Information = model.Information,
            Joined = model.Joined,
            Regions = model.Regions,

        };
    }
}
