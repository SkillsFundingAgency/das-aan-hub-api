using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryValidator : AbstractValidator<GetMemberActivitiesQuery>
{
    public GetMemberActivitiesQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new MemberIdValidator(membersReadRepository));
    }
}
