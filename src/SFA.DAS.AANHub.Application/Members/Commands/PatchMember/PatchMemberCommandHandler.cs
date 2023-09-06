using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using System.Text.Json;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Members.Commands.PatchMember;

public class PatchMemberCommandHandler : IRequestHandler<PatchMemberCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IMembersWriteRepository _membersWriteRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;

    public PatchMemberCommandHandler(IMembersWriteRepository membersWriteRepository, IDateTimeProvider dateTimeProvider,
        IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext, INotificationsWriteRepository notificationsWriteRepository)
    {
        _membersWriteRepository = membersWriteRepository;
        _dateTimeProvider = dateTimeProvider;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
        _notificationsWriteRepository = notificationsWriteRepository;
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

        if (member.Status.ToLower() == Constants.MembershipStatus.Withdrawn) { CreateNotification(member); }

        await _aanDataContext.SaveChangesAsync(cancellationToken);
        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));
    }

    private void CreateNotification(Member member)
    {
        var emailTemplate = member.UserType switch
        {
            "Apprentice" => EmailTemplateName.ApprenticeWithdrawal,
            "Employer" => EmailTemplateName.EmployerWithdrawal,
            _ => throw new NotImplementedException()
        };

        var tokens = GetTokens(member);
        Notification notification = NotificationHelper.CreateNotification(Guid.NewGuid(), member.Id, emailTemplate, tokens, member.Id, true, null);
        _notificationsWriteRepository.Create(notification);
    }

    private static string GetTokens(Member member)
    {
        var emailTemplate = new OptingOutEmailTemplate(member.FirstName, member.LastName);
        return JsonSerializer.Serialize(emailTemplate);
    }
}
