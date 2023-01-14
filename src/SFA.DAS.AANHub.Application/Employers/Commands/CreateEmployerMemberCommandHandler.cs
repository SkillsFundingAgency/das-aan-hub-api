using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandHandler : IRequestHandler<CreateEmployerMemberCommand, CreateEmployerMemberCommandResponse>
    {
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;

        public CreateEmployerMemberCommandHandler(IMembersWriteRepository membersWriteRepository,
            IAanDataContext aanDataContext, IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<CreateEmployerMemberCommandResponse> Handle(CreateEmployerMemberCommand command,
            CancellationToken cancellationToken)
        {
            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.RequestedByUserId ?? Guid.Empty,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member),
                Resource = MembershipUserType.Employer
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return member;
        }
    }
}
