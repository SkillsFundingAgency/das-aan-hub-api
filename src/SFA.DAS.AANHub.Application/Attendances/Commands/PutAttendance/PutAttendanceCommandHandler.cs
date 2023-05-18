using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommandHandler : IRequestHandler<PutAttendanceCommand, ValidatedResponse<PutCommandResult>>
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

    public async Task<ValidatedResponse<PutCommandResult>> Handle(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        var existingAttendance = await _attendancesWriteRepository.GetAttendance(command.CalendarEventId, command.RequestedByMemberId);

        if (existingAttendance != null)
        {
            if (existingAttendance.IsActive == command.RequestedActiveStatus)
            {
                return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
            }

            await _attendancesWriteRepository.SetActiveStatus(command.CalendarEventId, command.RequestedByMemberId, command.RequestedActiveStatus);

            CreateAudit(command);

            await _aanDataContext.SaveChangesAsync(cancellationToken);

            return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
        }
        else
        {
            if (command.RequestedActiveStatus == true)
            {
                CreateNewAttendance(command);

                CreateAudit(command);

                await _aanDataContext.SaveChangesAsync(cancellationToken);

                return new ValidatedResponse<PutCommandResult>(new PutCommandResult(true));
            }

            return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
        }
    }

    private void CreateNewAttendance(PutAttendanceCommand command)
    {
        Attendance newAttendance = command;
        _attendancesWriteRepository.Create(newAttendance);
    }

    private void CreateAudit(PutAttendanceCommand command)
    {
        _auditWriteRepository.Create(new Audit
        {
            Action = "Put",
            ActionedBy = command.RequestedByMemberId,
            AuditTime = DateTime.UtcNow,
            After = JsonSerializer.Serialize((Attendance)command),
            Resource = nameof(Attendance),
        });
    }
}
