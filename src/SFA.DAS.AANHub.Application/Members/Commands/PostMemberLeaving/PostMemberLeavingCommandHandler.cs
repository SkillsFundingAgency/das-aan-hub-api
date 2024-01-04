using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;

public class PostMemberLeavingCommandHandler : IRequestHandler<PostMemberLeavingCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;
    private readonly IMemberLeavingReasonsWriteRepository _memberLeavingReasonsWriteRepository;
    private readonly ILeavingReasonsReadRepository _leavingReasonsReadRepository;

    public PostMemberLeavingCommandHandler(IMembersWriteRepository membersWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext, INotificationsWriteRepository notificationsWriteRepository, IMemberLeavingReasonsWriteRepository memberLeavingReasonsWriteRepository, ILeavingReasonsReadRepository leavingReasonsReadRepository)
    {
        _membersWriteRepository = membersWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
        _notificationsWriteRepository = notificationsWriteRepository;
        _memberLeavingReasonsWriteRepository = memberLeavingReasonsWriteRepository;
        _leavingReasonsReadRepository = leavingReasonsReadRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PostMemberLeavingCommand command,
        CancellationToken cancellationToken)
    {
        var member = await _membersWriteRepository.Get(command.MemberId);

        var audit = new Audit()
        {
            Action = AuditAction.Withdrawn,
            Before = JsonSerializer.Serialize(member),
            ActionedBy = command.MemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Member),
            EntityId = command.MemberId,
        };

        member!.Status = "Withdrawn";
        member.EndDate = DateTime.UtcNow;
        member.LastUpdatedDate = DateTime.UtcNow;

        audit.After = JsonSerializer.Serialize(member);

        var validLeavingReasonIds = await _leavingReasonsReadRepository.GetAllLeavingReasons(cancellationToken);

        foreach (var leavingReasonId in command.LeavingReasons.Distinct().Where(leavingReasonId => validLeavingReasonIds.Exists(x => x.Id == leavingReasonId)))
        {
            _memberLeavingReasonsWriteRepository.Create(new MemberLeavingReason { Id = Guid.NewGuid(), LeavingReasonId = leavingReasonId, MemberId = member.Id });
        }

        _auditWriteRepository.Create(audit);

        CreateNotificationForWithdrawn(member);

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private void CreateNotificationForWithdrawn(Member member)
    {
        var templateName = member.UserType == UserType.Apprentice
            ? Domain.Common.Constants.EmailTemplateName.ApprenticeWithdrawal
            : Domain.Common.Constants.EmailTemplateName.EmployerWithdrawal;

        var tokens = GetTokens(member.FullName);
        var notification = NotificationHelper.CreateNotification(Guid.NewGuid(), member.Id, templateName, tokens, member.Id, true, member.Id.ToString());
        _notificationsWriteRepository.Create(notification);
    }

    private static string GetTokens(string fullName)
    {
        var tokens = new Dictionary<string, string>
        {
            { "contact", fullName },
        };

        return JsonSerializer.Serialize(tokens);
    }
}