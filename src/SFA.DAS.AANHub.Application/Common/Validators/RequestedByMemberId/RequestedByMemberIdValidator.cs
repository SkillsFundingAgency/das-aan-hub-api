using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId
{
    public class RequestedByMemberIdValidator : AbstractValidator<IRequestedByMemberId>
    {
        private const string RequestedByUserHeaderEmptyErrorMessage = "X-RequestedByUser header is empty";
        private const string RequestedByUserIdNotFoundMessage = "Could not find a valid active user ID matching the X-RequestedByUser header";
        public RequestedByMemberIdValidator(IMembersReadRepository membersReadRepository) => RuleFor(x => x.RequestedByMemberId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(RequestedByUserHeaderEmptyErrorMessage)
                .MustAsync(async (requestedByMemberId, cancellation) =>
                {
                    var member = await membersReadRepository.GetMember(requestedByMemberId);
                    return member is { Status: MembershipStatus.Live };
                })
                .WithMessage(RequestedByUserIdNotFoundMessage);
    }
}
