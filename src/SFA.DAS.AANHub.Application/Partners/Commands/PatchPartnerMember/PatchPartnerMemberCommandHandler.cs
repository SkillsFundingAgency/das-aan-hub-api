using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember
{
    public class PatchPartnerMemberCommandHandler : IRequestHandler<PatchPartnerMemberCommand, ValidatedResponse<PatchMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IPartnersWriteRepository _partnersWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PatchPartnerMemberCommandHandler(IPartnersWriteRepository partnersWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext,
            IDateTimeProvider dateTimeProvider)
        {
            _partnersWriteRepository = partnersWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ValidatedResponse<PatchMemberCommandResponse>> Handle(PatchPartnerMemberCommand command,
            CancellationToken cancellationToken)
        {
            var partner = await
                _partnersWriteRepository.GetPatchPartner(command.UserName!);

            if (partner == null)
                return new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));

            var audit = new Audit
            {
                Action = "Patch",
                ActionedBy = command.RequestedByMemberId,
                AuditTime = _dateTimeProvider.Now,
                Before = JsonSerializer.Serialize(partner),
                Resource = MembershipUserType.Partner
            };

            partner.LastUpdated = _dateTimeProvider.Now;

            command.PatchDoc.ApplyTo(partner);

            audit.After = JsonSerializer.Serialize(partner);

            _auditWriteRepository.Create(audit);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(true));
        }
    }
}
