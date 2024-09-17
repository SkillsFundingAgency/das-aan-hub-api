using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using System.Text.Json;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommandHandler : IRequestHandler<PutAttendanceCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IAanDataContext _aanDataContext;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;
    private readonly IMembersReadRepository _membersReadRepository;
    private readonly ICalendarEventsReadRepository _calendarEventReadRepository;
    private readonly INotificationsWriteRepository _notificationsWriteRepository;

    public PutAttendanceCommandHandler(IAanDataContext aanDataContext,
                                       IAuditWriteRepository auditWriteRepository,
                                       IAttendancesWriteRepository attendancesWriteRepository,
                                       IMembersReadRepository membersReadRepository,
                                       ICalendarEventsReadRepository calendarEventsReadRepository,
                                       INotificationsWriteRepository notificationsWriteRepository)
    {
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
        _attendancesWriteRepository = attendancesWriteRepository;
        _membersReadRepository = membersReadRepository;
        _calendarEventReadRepository = calendarEventsReadRepository;
        _notificationsWriteRepository = notificationsWriteRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        var existingAttendance = await _attendancesWriteRepository.GetAttendance(command.CalendarEventId, command.RequestedByMemberId);

        return existingAttendance switch
        {
            null when !command.IsAttending => new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()),
            not null when command.IsAttending == existingAttendance!.IsAttending => new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()),
            null => await CreateAttendance(command, cancellationToken),
            not null => await UpdateAttendance(existingAttendance, command, cancellationToken),
        };
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> UpdateAttendance(
        Attendance existingAttendance,
        PutAttendanceCommand command,
        CancellationToken token)
    {
        var audit = new Audit()
        {
            Action = AuditAction.Put,
            Before = JsonSerializer.Serialize(existingAttendance),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Attendance),
            EntityId = existingAttendance.CalendarEventId
        };

        existingAttendance.IsAttending = command.IsAttending;
        existingAttendance.CancelledDate = command.IsAttending ? null : DateTime.UtcNow;

        audit.After = JsonSerializer.Serialize(existingAttendance);
        _auditWriteRepository.Create(audit);

        await CreateNotification(command);

        await _aanDataContext.SaveChangesAsync(token);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> CreateAttendance(PutAttendanceCommand command, CancellationToken token)
    {
        Attendance newAttendance = command;

        var audit = new Audit()
        {
            Action = AuditAction.Create,
            After = JsonSerializer.Serialize(newAttendance),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Attendance),
            EntityId = command.CalendarEventId
        };


        _attendancesWriteRepository.Create(newAttendance);

        _auditWriteRepository.Create(audit);
        await CreateNotification(command);

        await _aanDataContext.SaveChangesAsync(token);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private async Task CreateNotification(PutAttendanceCommand command)
    {
        var member = await _membersReadRepository.GetMember(command.RequestedByMemberId);
        var templateName = GetTemplateName(command, member!);
        var tokens = await GetTokens(command, member!);

        Notification notification = NotificationHelper.CreateNotification(Guid.NewGuid(), command.RequestedByMemberId, templateName, tokens, command.RequestedByMemberId, true, command.CalendarEventId.ToString());
        _notificationsWriteRepository.Create(notification);
    }

    private static string GetTemplateName(PutAttendanceCommand command, Member member)
    {
        return (command.IsAttending, member.UserType) switch
        {
            (true, UserType.Apprentice) => EmailTemplateName.ApprenticeEventSignUpTemplate,
            (false, UserType.Apprentice) => EmailTemplateName.ApprenticeEventCancelTemplate,
            (true, UserType.Employer) => EmailTemplateName.EmployerEventSignUpTemplate,
            (false, UserType.Employer) => EmailTemplateName.EmployerEventCancelTemplate,
            _ => throw new NotImplementedException()
        };
    }

    private async Task<string> GetTokens(PutAttendanceCommand command, Member member)
    {
        var calendarEvent = await _calendarEventReadRepository.GetCalendarEvent(command.CalendarEventId);
        var startDate = calendarEvent!.StartDate.UtcToLocalTime();

        var date = startDate.ToString("dd/MM/yyyy");
        var time = startDate.ToString("HH:mm");

        var emailTemplate = new EventAttendanceEmailTemplate(member.FirstName, member.LastName, calendarEvent!.Title, date, time);
        return JsonSerializer.Serialize(emailTemplate);
    }
}
