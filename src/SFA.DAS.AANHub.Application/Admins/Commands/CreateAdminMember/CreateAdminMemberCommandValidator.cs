using FluentValidation;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandValidator : AbstractValidator<CreateAdminMemberCommand>
{
    public CreateAdminMemberCommandValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new CreateMemberCommandBaseValidator(membersReadRepository));
    }
}