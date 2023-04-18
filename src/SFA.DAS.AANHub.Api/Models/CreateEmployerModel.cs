using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;

namespace SFA.DAS.AANHub.Api.Models
{
    public class CreateEmployerModel
    {
        public long AccountId { get; set; }
        public Guid UserRef { get; set; }
        public string Organisation { get; set; } = null!;
        public DateTime Joined { get; set; }
        public int? RegionId { get; set; }
        public string? Information { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;

        public static implicit operator CreateEmployerMemberCommand(CreateEmployerModel model) => new()
        {
            AccountId = model.AccountId,
            UserRef = model.UserRef,
            Organisation = model.Organisation,
            Email = model.Email,
            Name = model.Name,
            Information = model.Information,
            Joined = model.Joined,
            RegionId = model.RegionId
        };
    }
}