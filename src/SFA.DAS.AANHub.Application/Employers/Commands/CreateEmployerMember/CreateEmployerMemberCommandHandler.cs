using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember
{
    public class CreateEmployerMemberCommandHandler :
        IRequestHandler<CreateEmployerMemberCommand, ValidatedResponse<CreateMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IMembersWriteRepository _membersWriteRepository;

        public CreateEmployerMemberCommandHandler(IMembersWriteRepository membersWriteRepository,
            IAanDataContext aanDataContext, IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<ValidatedResponse<CreateMemberCommandResponse>> Handle(CreateEmployerMemberCommand command,
            CancellationToken cancellationToken)
        {
            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.MemberId,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member.Employer),
                Resource = MembershipUserType.Employer
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(member.Id));
        }
    }
}