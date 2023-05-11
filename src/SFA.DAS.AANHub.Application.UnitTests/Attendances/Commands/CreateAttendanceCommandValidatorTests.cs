using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Commands;

public class CreateAttendanceCommandValidatorTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Validation_MissingRequestedByMemberIdHeader_Fails(IMembersReadRepository membersReadRepository)
    {
        var futureCalendarEvent = new CalendarEvent() { Id = Guid.NewGuid(), StartDate = DateTime.UtcNow.AddDays(2) };

        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(futureCalendarEvent.Id))
                                    .ReturnsAsync(futureCalendarEvent);

        var command = new CreateAttendanceCommand(futureCalendarEvent.Id, Guid.Empty);

        var sut = new CreateAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository);

        var result = await sut.TestValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Validation_UnknownCalendarEventId_Fails(Member member, CalendarEvent calendarEvent)
    {
        var membersReadRepository = new Mock<IMembersReadRepository>();
        membersReadRepository.Setup(m => m.GetMember(member.Id))
                             .ReturnsAsync(member);

        CalendarEvent? nullCalendarEvent = null;
        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(calendarEvent.Id))
                                    .ReturnsAsync(nullCalendarEvent);

        var command = new CreateAttendanceCommand(calendarEvent.Id, member.Id);
        var sut = new CreateAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == CreateAttendanceCommandValidator.EventNotFoundMessage);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Validation_CalendarEventStartDateInPast_Fails(Member validMember, CalendarEvent pastCalendarEvent)
    {
        validMember.Status = Domain.Common.Constants.MembershipStatus.Live;

        var membersReadRepository = new Mock<IMembersReadRepository>();
        membersReadRepository.Setup(m => m.GetMember(validMember.Id))
                             .ReturnsAsync(validMember);

        pastCalendarEvent.StartDate = new DateTime(1990, 12, 31);

        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(pastCalendarEvent.Id))
                                    .ReturnsAsync(pastCalendarEvent);

        var command = new CreateAttendanceCommand(pastCalendarEvent.Id, validMember.Id);
        var sut = new CreateAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.Errors.Should().Contain(e => e.ErrorMessage == CreateAttendanceCommandValidator.EventInPastMessage);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Validation_ActiveCalendarEventWithStartDateInFuture_Succeeds(Member validMember, CalendarEvent futureCalendarEvent)
    {
        validMember.Status = Domain.Common.Constants.MembershipStatus.Live;
        futureCalendarEvent.IsActive = true;

        var membersReadRepository = new Mock<IMembersReadRepository>();
        membersReadRepository.Setup(m => m.GetMember(validMember.Id))
                             .ReturnsAsync(validMember);

        futureCalendarEvent.StartDate = DateTime.UtcNow.AddDays(2);

        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(futureCalendarEvent.Id))
                                    .ReturnsAsync(futureCalendarEvent);

        var command = new CreateAttendanceCommand(futureCalendarEvent.Id, validMember.Id);
        var sut = new CreateAttendanceCommandValidator(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
