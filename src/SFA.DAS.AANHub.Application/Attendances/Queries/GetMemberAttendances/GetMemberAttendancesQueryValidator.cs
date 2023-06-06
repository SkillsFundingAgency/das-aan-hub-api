using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public class GetMemberAttendancesQueryValidator : AbstractValidator<GetMemberAttendancesQuery>
{
    public const string FromDateIsRequired = "FromDate must have a value";
    public const string ToDateIsRequired = "ToDate must have a value";
    public const string IncorrectDateRange = "ToDate must be greater than FromDate";
    public GetMemberAttendancesQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new RequestedByMemberIdValidator(membersReadRepository));

        RuleFor(r => r.FromDate)
            .NotEmpty()
            .WithMessage(FromDateIsRequired);
        RuleFor(r => r.ToDate)
            .NotEmpty()
            .WithMessage(ToDateIsRequired)
            .GreaterThanOrEqualTo(r => r.FromDate)
            .WithMessage(IncorrectDateRange);
    }
}
