using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommandHandler : IRequestHandler<CreateAdminMemberCommand, ValidatableResponse<CreateAdminMemberCommandResponse>>
    {
        private readonly IMembersWriteRepository _membersWriteRepository;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAanDataContext _aanDataContext;

        public CreateAdminMemberCommandHandler(IMembersWriteRepository membersWriteRepository,
            IAanDataContext aanDataContext, IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<ValidatableResponse<CreateAdminMemberCommandResponse>> Handle(CreateAdminMemberCommand command,
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
                Resource = MembershipUserType.Admin
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatableResponse<CreateAdminMemberCommandResponse>(member);
        }
    }
}
