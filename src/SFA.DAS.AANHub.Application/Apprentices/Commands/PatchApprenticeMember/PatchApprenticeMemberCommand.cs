using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommand : PatchMemberCommandBase<Apprentice>, IRequest<ValidatedResponse<PatchMemberCommandResponse>>,
        IRequestedByMemberId
    {
        private const string EmailIdentifier = "Email";
        private const string NameIdentifier = "Name";
        public Guid ApprenticeId { get; set; }

        public string? Email =>
            GetReplacementValue(PatchDoc, EmailIdentifier);

        public string? Name =>
            GetReplacementValue(PatchDoc, NameIdentifier);

        public bool IsPresentEmail => HasValue(PatchDoc, EmailIdentifier);

        public bool IsPresentName => HasValue(PatchDoc, NameIdentifier);

        public Guid RequestedByMemberId { get; set; }
    }
}