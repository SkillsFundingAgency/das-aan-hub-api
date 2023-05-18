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
    private readonly IAttendancesReadRepository _attendancesReadRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;

                                       
    public PutAttendanceCommandHandler(IAanDataContext aanDataContext,
                                       IAuditWriteRepository auditWriteRepository,
                                       IAttendancesReadRepository attendancesReadRepository,
                                       IAttendancesWriteRepository attendancesWriteRepository)
    {
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
        _attendancesReadRepository = attendancesReadRepository;
        _attendancesWriteRepository = attendancesWriteRepository;
    }

    public async Task<ValidatedResponse<PutCommandResult>> Handle(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        var existingAttendance = await _attendancesReadRepository.GetAttendance(command.CalendarEventId, command.RequestedByMemberId);

        if (existingAttendance != null)
        {
            if (existingAttendance.IsActive == command.RequestedActiveStatus)
            {
                return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
            }

            await ChangeActiveStatus(command, cancellationToken);

            CreateAudit(command);

            return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
        }
        else
        {
            if (command.RequestedActiveStatus == true)
            {
                await CreateNewAttendance(command, cancellationToken);

                return new ValidatedResponse<PutCommandResult>(new PutCommandResult(true));
            }

            return new ValidatedResponse<PutCommandResult>(new PutCommandResult(false));
        }
    }

    private async Task CreateNewAttendance(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        Attendance newAttendance = command;

        _attendancesWriteRepository.Create(newAttendance);

        await _aanDataContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ChangeActiveStatus(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        _attendancesWriteRepository.SetActiveStatus(command.CalendarEventId, command.RequestedByMemberId, command.RequestedActiveStatus);

        await _aanDataContext.SaveChangesAsync(cancellationToken);
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
