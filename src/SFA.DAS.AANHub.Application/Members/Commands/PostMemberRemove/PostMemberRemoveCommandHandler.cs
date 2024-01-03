using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;

public class PostMemberRemoveCommandHandler : IRequestHandler<PostMemberRemoveCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;

    public PostMemberRemoveCommandHandler(IMembersWriteRepository membersWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext, INotificationsWriteRepository notificationsWriteRepository)
    {
        _membersWriteRepository = membersWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
        _notificationsWriteRepository = notificationsWriteRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PostMemberRemoveCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _membersWriteRepository.Get(request.MemberId);

        var audit = new Audit()
        {
            Action = AuditAction.Post,
            Before = JsonSerializer.Serialize(member),
            ActionedBy = request.AdminMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Member),
            EntityId = member!.Id
        };

        member.Status = request.Status.ToString();
        member.EndDate = DateTime.UtcNow;
        member.LastUpdatedDate = DateTime.UtcNow;

        audit.After = JsonSerializer.Serialize(member);
        _auditWriteRepository.Create(audit);

        if (request.Status == MembershipStatusType.Removed)
        {
            CreateNotificationForRemoved(request, member);
        }

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private void CreateNotificationForRemoved(PostMemberRemoveCommand command, Member member)
    {
        const string templateName = Domain.Common.Constants.EmailTemplateName.AANMemberRemoved;
        var tokens = GetTokens(member.FullName);
        var notification = NotificationHelper.CreateNotification(Guid.NewGuid(), command.MemberId, templateName, tokens, command.AdminMemberId, true, command.MemberId.ToString());
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