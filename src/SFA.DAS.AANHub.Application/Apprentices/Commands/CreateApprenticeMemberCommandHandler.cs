using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeMemberCommandHandler : IRequestHandler<CreateApprenticeMemberCommand, CreateApprenticeMemberCommandResponse>
    {
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;

        public CreateApprenticeMemberCommandHandler(IMembersWriteRepository membersWriteRepository, IAanDataContext aanDataContext,
            IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<CreateApprenticeMemberCommandResponse> Handle(CreateApprenticeMemberCommand command,
            CancellationToken cancellationToken)
        {

            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.RequestedByMemberId ?? Guid.Empty,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member),
                Resource = MembershipUserType.Apprentice.ToString()
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return member;
        }
    }

}
