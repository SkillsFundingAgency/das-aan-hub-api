using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId
{
    public class RequestedByMemberIdValidator : AbstractValidator<IRequestedByMemberId>
    {
        private const string RequestedByMemberIdEmptyErrorMessage = "RequestedByMemberId is empty";
        private const string RequestedByMemberIdNotFoundMessage = "RequestedByMemberId was not found";
        public RequestedByMemberIdValidator(IMembersReadRepository membersReadRepository) => RuleFor(x => x.RequestedByMemberId)
                .NotEmpty()
                .WithMessage(RequestedByMemberIdEmptyErrorMessage)
                .MustAsync(async (requestedByMemberId, cancellation) =>
                {
                    var member = await membersReadRepository.GetMember(requestedByMemberId);
                    return member is { Status: MembershipStatus.Live };
                })
                .WithMessage(RequestedByMemberIdNotFoundMessage);
    }
}
