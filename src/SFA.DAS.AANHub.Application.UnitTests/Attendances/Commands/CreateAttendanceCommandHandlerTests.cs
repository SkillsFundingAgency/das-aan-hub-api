using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands;

public class CreateAttendanceCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsResponseWithAttendanceId(
        CreateAttendanceCommandHandler sut,
        CreateAttendanceCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());
        response.Result.AttendanceId.Should().Be(command.Id);
    }

    [Test, MoqAutoData]
    public async Task Handle_CreatesNewAttendance(
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        CreateAttendanceCommandHandler sut,
        CreateAttendanceCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());

        attendancesWriteRepository.Verify(p => p.Create(It.Is<Attendance>(x =>
            x.Id == command.Id
            && x.CalendarEventId == command.CalendarEventId
            && x.MemberId == command.RequestedByMemberId
        )));
    }

    [Test, MoqAutoData]
    public async Task Handle_CreatesAudit(
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        CreateAttendanceCommandHandler sut,
        CreateAttendanceCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());

        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
    }
}
