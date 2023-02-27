using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember
{
    public class PatchEmployerMemberCommand : PatchMemberCommandBase<Employer>, IRequest<ValidatedResponse<PatchMemberCommandResponse>>, IRequestedByMemberId
    {
        public const string EmailIdentifier = "Email";
        public const string NameIdentifier = "Name";
        public const string OrganisationIdentifier = "Organisation";

        public Guid UserRef { get; set; }

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
