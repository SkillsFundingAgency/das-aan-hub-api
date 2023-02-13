using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandHandler : IRequestHandler<PatchApprenticeMemberCommand, ValidatedResponse<PatchMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IApprenticesWriteRepository _apprenticesWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;

        public PatchApprenticeMemberCommandHandler(IApprenticesWriteRepository apprenticesWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext)
        {
            _apprenticesWriteRepository = apprenticesWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
        }

        public async Task<ValidatedResponse<PatchMemberCommandResponse>> Handle(PatchApprenticeMemberCommand command,
            CancellationToken cancellationToken)
        {
            var apprentice = await
                _apprenticesWriteRepository.GetPatchApprentice(command.ApprenticeId);

            if (apprentice == null)
                return new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));

            var audit = new Audit
            {
                Action = "Patch",
                ActionedBy = command.RequestedByMemberId,
                AuditTime = DateTime.UtcNow,
                Before = JsonSerializer.Serialize(apprentice),
                Resource = MembershipUserType.Apprentice
            };

            command.PatchDoc.ApplyTo(apprentice);

            audit.After = JsonSerializer.Serialize(apprentice);

            _auditWriteRepository.Create(audit);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(true));
        }
    }
}