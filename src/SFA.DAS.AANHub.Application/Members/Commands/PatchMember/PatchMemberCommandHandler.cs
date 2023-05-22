using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

public class PatchMemberCommandHandler : IRequestHandler<PatchMemberCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;

    public PatchMemberCommandHandler(IMembersWriteRepository membersWriteRepository, IDateTimeProvider dateTimeProvider, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext)
    {
        _membersWriteRepository = membersWriteRepository;
        _dateTimeProvider = dateTimeProvider;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PatchMemberCommand command, CancellationToken cancellationToken)
    {
        var member = await _membersWriteRepository.Get(command.MemberId);

        if (member == null)
            return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));

        var audit = new Audit()
        {
            Action = "PatchMember",
            ActionedBy = command.MemberId,
            AuditTime = _dateTimeProvider.Now,
            Before = JsonSerializer.Serialize(member),
            Resource = nameof(Member),
        };

        member.LastUpdatedDate = _dateTimeProvider.Now;

        command.PatchDoc.ApplyTo(member);

        audit.After = JsonSerializer.Serialize(member);

        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));
    }
}
