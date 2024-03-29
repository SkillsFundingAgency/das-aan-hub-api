﻿using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandHandler : IRequestHandler<CreateAdminMemberCommand, ValidatedResponse<CreateMemberCommandResponse>>
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

    public async Task<ValidatedResponse<CreateMemberCommandResponse>> Handle(CreateAdminMemberCommand command,
        CancellationToken cancellationToken)
    {
        Member member = command;

        _membersWriteRepository.Create(member);

        _auditWriteRepository.Create(new Audit
        {
            Action = AuditAction.Create,
            ActionedBy = command.MemberId,
            AuditTime = DateTime.UtcNow,
            After = JsonSerializer.Serialize(member),
            Resource = nameof(UserType.Admin),
            EntityId = member.Id
        });

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(member.Id));
    }
}