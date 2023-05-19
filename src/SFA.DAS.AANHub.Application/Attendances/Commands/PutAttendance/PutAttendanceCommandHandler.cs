using MediatR;
using Newtonsoft.Json.Linq;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommandHandler : IRequestHandler<PutAttendanceCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IAanDataContext _aanDataContext;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;
                                       
    public PutAttendanceCommandHandler(IAanDataContext aanDataContext,
                                       IAuditWriteRepository auditWriteRepository,
                                       IAttendancesWriteRepository attendancesWriteRepository)
    {
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
        _attendancesWriteRepository = attendancesWriteRepository;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        var existingAttendance = await _attendancesWriteRepository.GetAttendance(command.CalendarEventId, command.RequestedByMemberId);

        if ((existingAttendance == null && !command.IsAttending) || (existingAttendance != null && command.IsAttending == existingAttendance.IsActive))
        {
            return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
        }

        var task = existingAttendance == null ? CreateAttendance(command, cancellationToken) : UpdateAttendance(existingAttendance, command, cancellationToken);

        return await task;
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> UpdateAttendance(
        Attendance existingAttendance, 
        PutAttendanceCommand command, 
        CancellationToken token)
    {
        var audit = new Audit()
        {
            Action = "Put",
            Before = JsonSerializer.Serialize(existingAttendance),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Attendance),
        };

        existingAttendance.IsActive = command.IsAttending;

        audit.After = JsonSerializer.Serialize(existingAttendance);
        _auditWriteRepository.Create(audit);

        await _aanDataContext.SaveChangesAsync(token);
        
        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> CreateAttendance(PutAttendanceCommand command, CancellationToken token)
    {
        Attendance newAttendance = command;

        var audit = new Audit()
        {
            Action = "Create",
            After = JsonSerializer.Serialize(newAttendance),
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            Resource = nameof(Attendance),
        };
        _auditWriteRepository.Create(audit);

        _attendancesWriteRepository.Create(newAttendance);

        await _aanDataContext.SaveChangesAsync(token);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }
}
