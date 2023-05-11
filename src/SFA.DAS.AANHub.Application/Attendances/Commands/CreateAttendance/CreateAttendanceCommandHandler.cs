using System.Text.Json;
using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;

public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand,
    ValidatedResponse<CreateAttendanceCommandResponse>>
{
    private readonly IAanDataContext _aanDataContext;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAttendancesWriteRepository _attendancesWriteRepository;

    public CreateAttendanceCommandHandler(IAttendancesWriteRepository attendancesWriteRepository, IAanDataContext aanDataContext,
        IAuditWriteRepository auditWriteRepository)
    {
        _attendancesWriteRepository = attendancesWriteRepository;
        _aanDataContext = aanDataContext;
        _auditWriteRepository = auditWriteRepository;
    }

    public async Task<ValidatedResponse<CreateAttendanceCommandResponse>> Handle(CreateAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        Attendance attendance = command;

        _attendancesWriteRepository.Create(attendance);

        _auditWriteRepository.Create(new Audit
        {
            Action = "Create",
            ActionedBy = command.MemberId,
            AuditTime = DateTime.UtcNow,
            After = JsonSerializer.Serialize(attendance), // TODO: Please check
            Resource = nameof(attendance),
        });

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<CreateAttendanceCommandResponse>(new CreateAttendanceCommandResponse(attendance.Id));
    }
}
