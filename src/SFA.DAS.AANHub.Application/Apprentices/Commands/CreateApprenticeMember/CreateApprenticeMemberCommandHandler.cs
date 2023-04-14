using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember
{
    public class CreateApprenticeMemberCommandHandler : IRequestHandler<CreateApprenticeMemberCommand,
        ValidatedResponse<CreateApprenticeMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IMembersWriteRepository _membersWriteRepository;

        public CreateApprenticeMemberCommandHandler(IMembersWriteRepository membersWriteRepository, IAanDataContext aanDataContext,
            IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<ValidatedResponse<CreateApprenticeMemberCommandResponse>> Handle(CreateApprenticeMemberCommand command,
            CancellationToken cancellationToken)
        {
            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.Id,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member.Apprentice),
                Resource = MembershipUserType.Apprentice
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<CreateApprenticeMemberCommandResponse>(member);
        }
    }
}