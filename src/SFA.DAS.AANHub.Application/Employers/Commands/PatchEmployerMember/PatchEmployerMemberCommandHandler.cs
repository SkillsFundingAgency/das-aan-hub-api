using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember
{
    public class PatchEmployerMemberCommandHandler : IRequestHandler<PatchEmployerMemberCommand, ValidatedResponse<PatchMemberCommandResponse>>
    {
        private readonly IEmployersWriteRepository _employersWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PatchEmployerMemberCommandHandler(IEmployersWriteRepository employersWriteRepository,
            IAuditWriteRepository auditWriteRepository,
            IAanDataContext aanDataContext,
            IDateTimeProvider dateTimeProvider)
        {
            _employersWriteRepository = employersWriteRepository;
            _auditWriteRepository = auditWriteRepository;
            _aanDataContext = aanDataContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ValidatedResponse<PatchMemberCommandResponse>> Handle(PatchEmployerMemberCommand command, CancellationToken cancellationToken)
        {
            var employer = await
                _employersWriteRepository.GetPatchEmployer(command.UserRef);

            if (employer == null)
                return new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));

            var audit = new Audit()
            {
                Action = "Patch",
                ActionedBy = command.RequestedByMemberId,
                AuditTime = _dateTimeProvider.Now,
                Before = JsonSerializer.Serialize(employer),
                Resource = MembershipUserType.Employer
            };

            employer.LastUpdated = _dateTimeProvider.Now;

            command.PatchDoc.ApplyTo(employer);

            audit.After = JsonSerializer.Serialize(employer);

            _auditWriteRepository.Create(audit);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(true));
        }
    }
}
