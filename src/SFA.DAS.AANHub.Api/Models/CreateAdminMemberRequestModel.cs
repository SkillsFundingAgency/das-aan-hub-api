using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

namespace SFA.DAS.AANHub.Api.Models;

public record CreateAdminMemberRequestModel(string Email, string FirstName, string LastName)
{
    public static implicit operator CreateAdminMemberCommand(CreateAdminMemberRequestModel model) => new()
    {
        Email = model.Email!,
        FirstName = model.FirstName!,
        LastName = model.LastName!,
        JoinedDate = DateTime.UtcNow,
        OrganisationName = "DFE"
    };
}
