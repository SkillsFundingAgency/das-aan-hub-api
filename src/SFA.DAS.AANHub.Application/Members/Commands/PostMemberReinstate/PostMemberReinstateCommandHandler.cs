using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;

public class PostMemberReinstateCommandHandler : IRequestHandler<PostMemberReinstateCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;
    private readonly IMemberLeavingReasonsWriteRepository _memberLeavingReasonsWriteRepository;

    public PostMemberReinstateCommandHandler(IMembersWriteRepository membersWriteRepository,
        IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext,
        IMemberLeavingReasonsWriteRepository memberLeavingReasonsWriteRepository,
        ILeavingReasonsReadRepository leavingReasonsReadRepository)
    {
        _membersWriteRepository = membersWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
        _memberLeavingReasonsWriteRepository = memberLeavingReasonsWriteRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PostMemberReinstateCommand command,
        CancellationToken cancellationToken)
    {
        var member = await _membersWriteRepository.Get(command.MemberId);

        var audit = new Audit()
        {
            Action = AuditAction.Reinstate,
            Before = JsonSerializer.Serialize(member),
            ActionedBy = command.MemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Member),
            EntityId = command.MemberId,
        };

        member!.Status = "Live";
        member.EndDate = null;
        member.LastUpdatedDate = DateTime.UtcNow;

        audit.After = JsonSerializer.Serialize(member);

        await _memberLeavingReasonsWriteRepository.DeleteLeavingReasons(command.MemberId, cancellationToken);
        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }
}
