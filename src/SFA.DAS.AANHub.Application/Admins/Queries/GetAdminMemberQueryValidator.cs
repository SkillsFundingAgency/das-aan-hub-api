using FluentValidation;

namespace SFA.DAS.AANHub.Application.Admins.Queries
{
    public class GetAdminMemberQueryValidator : AbstractValidator<GetAdminMemberQuery>
    {
        public GetAdminMemberQueryValidator()
        {
            RuleFor(a => a.UserName)
                .NotEmpty();
        }
    }
}