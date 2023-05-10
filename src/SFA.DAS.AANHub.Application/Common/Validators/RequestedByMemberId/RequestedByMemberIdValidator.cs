using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId
{
    public class RequestedByMemberIdValidator : AbstractValidator<IRequestedByMemberId>
    {
        private const string RequestedByMemberHeaderEmptyErrorMessage = "X-RequestedByMemberId header is empty";
        private const string RequestedByMemberIdNotFoundMessage = "Could not find a valid active Member ID matching the X-RequestedByMemberId header";
        public RequestedByMemberIdValidator(IMembersReadRepository membersReadRepository) => RuleFor(x => x.RequestedByMemberId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(RequestedByMemberHeaderEmptyErrorMessage)
                .MustAsync(async (requestedByMemberId, cancellation) =>
                {
                    var member = await membersReadRepository.GetMember(requestedByMemberId);
                    return member is { Status: MembershipStatus.Live };
                })
                .WithMessage(RequestedByMemberIdNotFoundMessage);
    }
}
