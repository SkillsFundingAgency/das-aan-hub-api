using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId
{
    public class RequestedByMemberIdValidator : AbstractValidator<IRequestedByMemberId>
    {
        public const string RequestedByMemberIdEmptyErrorMessage = "RequestedByMemberId is empty";
        public const string RequestedByMemberIdNotFoundMessage = "RequestedByMemberId was not found";
        public RequestedByMemberIdValidator(IMembersReadRepository membersReadRepository)
        {
            RuleFor(x => x.RequestedByMemberId)
                .NotNull()
                .NotEmpty()
                .WithMessage(RequestedByMemberIdEmptyErrorMessage)
                .MustAsync(async (RequestedByMemberId, cancellation) =>
                {
                    var member = await membersReadRepository.GetMember(RequestedByMemberId);
                    return member != null && member.Status == MembershipStatus.Live;
                })
                .WithMessage(RequestedByMemberIdNotFoundMessage);
        }
    }
}
