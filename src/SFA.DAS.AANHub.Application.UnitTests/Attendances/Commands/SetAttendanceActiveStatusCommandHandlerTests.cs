using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands
{
    public class SetAttendanceActiveStatusCommandHandlerTests
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_AttendanceExists_RequestedActiveStatusIsSame_CreatesNothing(
            [Frozen] Mock<IAanDataContext> aanDataContext,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
            [Frozen] PutAttendanceCommandHandler sut,
            [Frozen] Attendance existingAttendance)
        {
            attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                      .ReturnsAsync(existingAttendance);

            var command = new PutAttendanceCommand(
                existingAttendance.CalendarEventId, 
                existingAttendance.MemberId, 
                existingAttendance.IsActive); 

            await sut.Handle(command, new CancellationToken());

            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);
            attendancesWriteRepository.Verify(a => a.SetActiveStatus(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Never);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_AttendanceExists_RequestedActiveStatusIsDifferent_UpdatesStatusAndAddsAudit(
            [Frozen] Mock<IAanDataContext> aanDataContext,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
            [Frozen] PutAttendanceCommandHandler sut,
            [Frozen] Attendance existingAttendance)
        {
            attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                      .ReturnsAsync(existingAttendance);

            bool differentStatus = !existingAttendance.IsActive;

            var command = new PutAttendanceCommand(
                existingAttendance.CalendarEventId,
                existingAttendance.MemberId,
                differentStatus);

            await sut.Handle(command, new CancellationToken());

            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);

            attendancesWriteRepository.Verify(a => a.SetActiveStatus(
                existingAttendance.CalendarEventId, 
                existingAttendance.MemberId,
                differentStatus), Times.Once);

            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            auditWriteRepository.Verify(a => a.Create(
                It.Is<Audit>(
                    a => a.Action == "Put"
                    && a.ActionedBy == command.RequestedByMemberId
                    && DateOnly.FromDateTime(a.AuditTime) == DateOnly.FromDateTime(DateTime.UtcNow)
                    && a.Resource == nameof(Attendance))), 
                        Times.Once);

        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_NoMatchingAttendance_RequestedActiveStatusIsTrue_CreatesAttendance(
            [Frozen] Mock<IAanDataContext> aanDataContext,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
            [Frozen] PutAttendanceCommandHandler sut,
            [Frozen] Attendance existingAttendance)
        {
            attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                      .ReturnsAsync(() => null);

            var command = new PutAttendanceCommand(
                existingAttendance.CalendarEventId,
                existingAttendance.MemberId,
                true);

            await sut.Handle(command, new CancellationToken());

            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Once);

            attendancesWriteRepository.Verify(a => a.SetActiveStatus(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>()), Times.Never);

            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);

        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_NoMatchingAttendance_RequestedActiveStatusIsFalse_CreatesNothing(
            [Frozen] Mock<IAanDataContext> aanDataContext,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
            [Frozen] PutAttendanceCommandHandler sut,
            [Frozen] Attendance existingAttendance)
        {
            attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                      .ReturnsAsync(() => null);

            var command = new PutAttendanceCommand(
                existingAttendance.CalendarEventId,
                existingAttendance.MemberId,
                false);

            await sut.Handle(command, new CancellationToken());

            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);

            attendancesWriteRepository.Verify(a => a.SetActiveStatus(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>()), Times.Never);

            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Never);

        }
    }
}
