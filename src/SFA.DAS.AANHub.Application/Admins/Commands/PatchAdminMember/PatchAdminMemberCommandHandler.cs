using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember
{
    public class PatchAdminMemberCommandHandler : IRequestHandler<PatchAdminMemberCommand, ValidatedResponse<PatchMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IAdminsWriteRepository _adminsWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PatchAdminMemberCommandHandler(IAdminsWriteRepository adminsWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext,
            IDateTimeProvider dateTimeProvider)
        {
            _adminsWriteRepository = adminsWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ValidatedResponse<PatchMemberCommandResponse>> Handle(PatchAdminMemberCommand command,
            CancellationToken cancellationToken)
        {
            var admin = await
                _adminsWriteRepository.GetPatchAdmin(command.UserName);

            if (admin == null)
                return new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));

            var audit = new Audit
            {
                Action = "Patch",
                ActionedBy = command.RequestedByMemberId,
                AuditTime = _dateTimeProvider.Now,
                Before = JsonSerializer.Serialize(admin),
                Resource = MembershipUserType.Admin
            };

            admin.LastUpdated = _dateTimeProvider.Now;

            command.PatchDoc.ApplyTo(admin);

            audit.After = JsonSerializer.Serialize(admin);

            _auditWriteRepository.Create(audit);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(true));
        }
    }
}