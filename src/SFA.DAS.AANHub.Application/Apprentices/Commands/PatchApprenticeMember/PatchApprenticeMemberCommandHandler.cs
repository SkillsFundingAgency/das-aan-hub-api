using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandHandler : IRequestHandler<PatchApprenticeMemberCommand, ValidatedResponse<PatchApprenticeMemberCommandResponse>>
    {
        private readonly IApprenticesWriteRepository _apprenticesWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;

        public PatchApprenticeMemberCommandHandler(IApprenticesWriteRepository apprenticesWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext)
        {
            _apprenticesWriteRepository = apprenticesWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
        }

        public async Task<ValidatedResponse<PatchApprenticeMemberCommandResponse>> Handle(PatchApprenticeMemberCommand command, CancellationToken cancellationToken)
        {
            var success = true;

            var apprentice = await
                _apprenticesWriteRepository.GetApprentice(command.ApprenticeId);

            if (apprentice == null)
                return new ValidatedResponse<PatchApprenticeMemberCommandResponse>(new PatchApprenticeMemberCommandResponse(false));

            var audit = new Audit()
            {
                Action = "Patch",
                ActionedBy = command.RequestedByMemberId,
                AuditTime = DateTime.UtcNow,
                Before = JsonSerializer.Serialize(apprentice),
                Resource = MembershipUserType.Apprentice
            };

            command.Patchdoc.ApplyTo(apprentice);

            audit.After = JsonSerializer.Serialize(apprentice);

            _auditWriteRepository.Create(audit);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PatchApprenticeMemberCommandResponse>
            (new PatchApprenticeMemberCommandResponse(success));
        }
    }
}
