﻿using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember
{
    public class CreateAdminMemberCommandHandler : IRequestHandler<CreateAdminMemberCommand, ValidatedResponse<CreateAdminMemberCommandResponse>>
    {
        private readonly IAanDataContext _aanDataContext;
        private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IMembersWriteRepository _membersWriteRepository;

        public CreateAdminMemberCommandHandler(IMembersWriteRepository membersWriteRepository,
            IAanDataContext aanDataContext, IAuditWriteRepository auditWriteRepository)
        {
            _membersWriteRepository = membersWriteRepository;
            _aanDataContext = aanDataContext;
            _auditWriteRepository = auditWriteRepository;
        }

        public async Task<ValidatedResponse<CreateAdminMemberCommandResponse>> Handle(CreateAdminMemberCommand command,
            CancellationToken cancellationToken)
        {
            Member member = command;

            _membersWriteRepository.Create(member);

            _auditWriteRepository.Create(new Audit
            {
                Action = "Create",
                ActionedBy = command.Id,
                AuditTime = DateTime.UtcNow,
                After = JsonSerializer.Serialize(member.Admin),
                Resource = MembershipUserType.Admin
            });

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<CreateAdminMemberCommandResponse>(member);
        }
    }
}