using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember
{
    public class PatchPartnerMemberCommand : PatchMemberCommandBase<Partner>, IRequest<ValidatedResponse<PatchMemberCommandResponse>>,
        IRequestedByMemberId
    {
        private const string EmailIdentifier = "Email";
        private const string NameIdentifier = "Name";
        public const string OrganisationIdentifier = "Organisation";

        public string UserName { get; set; } = null!;

        public string? Email =>
            GetReplacementValue(PatchDoc, EmailIdentifier);

        public string? Name =>
            GetReplacementValue(PatchDoc, NameIdentifier);

        public string? Organisation =>
            GetReplacementValue(PatchDoc, OrganisationIdentifier);


        public bool IsPresentEmail => HasValue(PatchDoc, EmailIdentifier);

        public bool IsPresentName => HasValue(PatchDoc, NameIdentifier);

        public bool IsPresentOrganisation => HasValue(PatchDoc, OrganisationIdentifier);

        public Guid RequestedByMemberId { get; set; }

    }
}
