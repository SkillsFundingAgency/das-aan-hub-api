using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.DeleteCalendarEvent;
public class DeleteCalendarEventCommandValidatorTests
{
    [Test]
    public async Task Validate_AllDetailsValid()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
        var command = GetCommand();
        var member = GetMember();
        var calendarEvent = GetCalendarEvent();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Validate_CalendarIdNotSupplied()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
        var command = GetCommand();
        command.CalendarEventId = Guid.Empty;

        var member = GetMember();
        var calendarEvent = GetCalendarEvent();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
            .WithErrorMessage(DeleteCalendarEventCommandValidator.EventIdNotSuppliedMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_EventDoesNotExist()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
        var command = GetCommand();
        var member = GetMember();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync((CalendarEvent)null!);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
            .WithErrorMessage(DeleteCalendarEventCommandValidator.EventNotFoundMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_CalendarEventNotActive()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
        var command = GetCommand();
        var member = GetMember();
        var calendarEvent = GetCalendarEvent();
        calendarEvent.IsActive = false;

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
            .WithErrorMessage(DeleteCalendarEventCommandValidator.EventNotActiveMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_CalendarEventIsPast()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();
        var command = GetCommand();
        var member = GetMember();
        var calendarEvent = GetCalendarEvent();
        calendarEvent.StartDate = DateTime.UtcNow.AddDays(-1);

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEventId)
            .WithErrorMessage(DeleteCalendarEventCommandValidator.EventInPastMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_MemberIdEmpty()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();

        var member = GetMember();
        member.Id = Guid.Empty;
        var calendarEvent = GetCalendarEvent();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);
        var command = new DeleteCalendarEventCommand(calendarEvent.Id, Guid.Empty);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId)
            .WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_MemberIdDoesNotExist()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();

        var calendarEvent = GetCalendarEvent();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync((Member?)null);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);
        var command = new DeleteCalendarEventCommand(calendarEvent.Id, Guid.Empty);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId)
            .WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_MemberIdNotLive()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        Mock<ICalendarEventsReadRepository> calendarEventsReadRepository = new();

        var member = GetMember();
        member.Status = "NotLive";
        var calendarEvent = GetCalendarEvent();

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        calendarEventsReadRepository.Setup(x => x.GetCalendarEvent(It.IsAny<Guid>())).ReturnsAsync(calendarEvent);

        DeleteCalendarEventCommandValidator sut = new(calendarEventsReadRepository.Object, membersReadRepository.Object);
        var command = new DeleteCalendarEventCommand(calendarEvent.Id, Guid.Empty);
        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.RequestedByMemberId)
            .WithErrorMessage(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage);
        result.Errors.Count.Should().Be(1);
    }

    private static CalendarEvent GetCalendarEvent()
    {
        return new CalendarEvent
        {
            IsActive = true,
            StartDate = DateTime.UtcNow.AddDays(1),
            Id = Guid.NewGuid()
        };
    }

    private static Member GetMember()
    {
        return new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };
    }

    public static DeleteCalendarEventCommand GetCommand()
    {
        return new DeleteCalendarEventCommand(Guid.NewGuid(), Guid.NewGuid());
    }
}
