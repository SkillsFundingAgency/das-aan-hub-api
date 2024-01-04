using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands;

public class PutAttendanceCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AttendanceExists_RequestedAttendingStatusIsSame_CreatesNothing(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
        [Frozen] PutAttendanceCommandHandler sut,
        [Frozen] Attendance existingAttendance)
    {
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(existingAttendance);

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            existingAttendance.IsAttending);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Never);
            membersReadRepository.Verify(a => a.GetMember(It.IsAny<Guid>()), Times.Never);
            calendarEventsReadRepository.Verify(a => a.GetCalendarEvent(It.IsAny<Guid>()), Times.Never);
            notificationWriteRepository.Verify(a => a.Create(It.IsAny<Notification>()), Times.Never);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData(UserType.Employer)]
    [RecursiveMoqInlineAutoData(UserType.Apprentice)]
    public async Task Handle_AttendanceExists_RequestedAttendingStatusIsDifferent_UpdatesStatusAndAddsAudit(
        UserType userType,
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
        PutAttendanceCommandHandler sut,
        Attendance existingAttendance,
        Member member,
        CalendarEvent calendarEvent)
    {
        member.Id = existingAttendance.MemberId;
        member.UserType = userType;
        calendarEvent.Id = existingAttendance.CalendarEventId;
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(existingAttendance);
        membersReadRepository.Setup(a => a.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(a => a.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        bool differentStatus = !existingAttendance.IsAttending;

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            differentStatus);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            auditWriteRepository.Verify(a => a.Create(
                It.Is<Audit>(
                    a => a.Action == AuditAction.Put
                    && !string.IsNullOrWhiteSpace(a.Before)
                    && !string.IsNullOrWhiteSpace(a.After)
                    && a.ActionedBy == command.RequestedByMemberId
                    && DateOnly.FromDateTime(a.AuditTime) == DateOnly.FromDateTime(DateTime.UtcNow)
                    && a.Resource == nameof(Attendance))),
                        Times.Once);
            membersReadRepository.Verify(a => a.GetMember(It.IsAny<Guid>()), Times.Once);
            calendarEventsReadRepository.Verify(a => a.GetCalendarEvent(It.IsAny<Guid>()), Times.Once);
            notificationWriteRepository.Verify(a => a.Create(It.IsAny<Notification>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData(UserType.Employer)]
    [RecursiveMoqInlineAutoData(UserType.Apprentice)]
    public async Task Handle_NoMatchingAttendance_RequestedAttendingStatusIsTrue_CreatesAttendanceAndAudit(
        UserType userType,
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
        [Frozen] PutAttendanceCommandHandler sut,
        [Frozen] Attendance existingAttendance,
        Member member,
        CalendarEvent calendarEvent)
    {
        member.Id = existingAttendance.MemberId;
        member.UserType = userType;
        calendarEvent.Id = existingAttendance.CalendarEventId;
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(() => null);
        membersReadRepository.Setup(a => a.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(a => a.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            true);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            auditWriteRepository.Verify(a => a.Create(
                It.Is<Audit>(
                    a => a.Action == AuditAction.Create
                    && a.Before == null
                    && !string.IsNullOrWhiteSpace(a.After)
                    && a.ActionedBy == command.RequestedByMemberId
                    && DateOnly.FromDateTime(a.AuditTime) == DateOnly.FromDateTime(DateTime.UtcNow)
                    && a.Resource == nameof(Attendance))),
                        Times.Once);
            membersReadRepository.Verify(a => a.GetMember(It.IsAny<Guid>()), Times.Once);
            calendarEventsReadRepository.Verify(a => a.GetCalendarEvent(It.IsAny<Guid>()), Times.Once);
            notificationWriteRepository.Verify(a => a.Create(It.IsAny<Notification>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_NoMatchingAttendance_RequestedAttendingStatusIsFalse_CreatesNothing(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
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

        using (new AssertionScope())
        {
            attendancesWriteRepository.Verify(a => a.Create(It.IsAny<Attendance>()), Times.Never);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Never);
            membersReadRepository.Verify(a => a.GetMember(It.IsAny<Guid>()), Times.Never);
            calendarEventsReadRepository.Verify(a => a.GetCalendarEvent(It.IsAny<Guid>()), Times.Never);
            notificationWriteRepository.Verify(a => a.Create(It.IsAny<Notification>()), Times.Never);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test]
    [RecursiveMoqInlineAutoData(EmailTemplateName.ApprenticeEventSignUpTemplate, UserType.Apprentice)]
    [RecursiveMoqInlineAutoData(EmailTemplateName.EmployerEventSignUpTemplate, UserType.Employer)]
    public async Task Handle_NoMatchingAttendance_RequestedAttendingIsTrue_GetCorrectEmailTemplate(
        string templateName,
        UserType userType,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
        PutAttendanceCommandHandler sut,
        Attendance existingAttendance,
        Member member,
        CalendarEvent calendarEvent)
    {
        existingAttendance.IsAttending = true;
        member.Id = existingAttendance.MemberId;
        member.UserType = userType;
        calendarEvent.Id = existingAttendance.CalendarEventId;
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(() => null);
        membersReadRepository.Setup(a => a.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(a => a.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            existingAttendance.IsAttending);

        await sut.Handle(command, new CancellationToken());

        notificationWriteRepository.Verify(a => a.Create(It.Is<Notification>(x => x.TemplateName.Equals(templateName))));
    }

    [Test]
    [RecursiveMoqInlineAutoData(EmailTemplateName.ApprenticeEventCancelTemplate, UserType.Apprentice)]
    [RecursiveMoqInlineAutoData(EmailTemplateName.EmployerEventCancelTemplate, UserType.Employer)]
    public async Task Handle_MatchingAttendanceFound_RequestedAttendingIsFalse_GetCorrectEmailTemplate(
        string templateName,
        UserType userType,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepository,
        PutAttendanceCommandHandler sut,
        Attendance existingAttendance,
        Member member,
        CalendarEvent calendarEvent)
    {
        existingAttendance.IsAttending = true;
        member.Id = existingAttendance.MemberId;
        member.UserType = userType;
        calendarEvent.Id = existingAttendance.CalendarEventId;
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(existingAttendance);
        membersReadRepository.Setup(a => a.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(a => a.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            false);

        await sut.Handle(command, new CancellationToken());

        notificationWriteRepository.Verify(a => a.Create(It.Is<Notification>(x => x.TemplateName.Equals(templateName))));
    }

    [Test]
    [RecursiveMoqInlineAutoData(UserType.Admin)]
    public async Task Handle_WhenUserTypeIsInvalid_GetNotImplementedException(
        UserType userType,
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAttendancesWriteRepository> attendancesWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
        PutAttendanceCommandHandler sut,
        Attendance existingAttendance,
        Member member,
        CalendarEvent calendarEvent)
    {
        existingAttendance.IsAttending = false;
        member.Id = existingAttendance.MemberId;
        member.UserType = userType;
        calendarEvent.Id = existingAttendance.CalendarEventId;
        attendancesWriteRepository.Setup(a => a.GetAttendance(existingAttendance.CalendarEventId, existingAttendance.MemberId))
                                  .ReturnsAsync(existingAttendance);
        membersReadRepository.Setup(a => a.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(a => a.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        var command = new PutAttendanceCommand(
            existingAttendance.CalendarEventId,
            existingAttendance.MemberId,
            true);

        Func<Task> result = async () => await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            await result.Should().ThrowAsync<NotImplementedException>();
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
