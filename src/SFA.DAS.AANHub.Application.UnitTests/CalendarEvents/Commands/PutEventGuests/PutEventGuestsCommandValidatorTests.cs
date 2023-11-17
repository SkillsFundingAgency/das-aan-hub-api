using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutEventGuests;

public class PutEventGuestsCommandValidatorTests
{
    [Test]
    public async Task Validate_AllDetailsValid()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Apprentice,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Validate_MemberNotLive()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Cancelled.ToString(),
            IsRegionalChair = false
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.AdminMemberId)
            .WithErrorMessage(PutEventGuestsCommandValidator.RequestedByMemberIdMustBeAdmin);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_MemberNotAdmin()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Apprentice,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = false
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.AdminMemberId)
            .WithErrorMessage(PutEventGuestsCommandValidator.RequestedByMemberIdMustBeAdmin);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_CalendarEventDoesNotExist()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        command.CalendarEvent = null;
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEvent)
            .WithErrorMessage(PutEventGuestsCommandValidator.CalendarEventDoesNotExist);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_CalendarEventIsNotActive()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        command.CalendarEvent!.IsActive = false;
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEvent)
            .WithErrorMessage(PutEventGuestsCommandValidator.CalendarEventIsNotActive);
        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task Validate_CalendarEventIsInPast()
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        command.CalendarEvent!.StartDate = DateTime.Now.AddHours(-1);
        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.CalendarEvent)
            .WithErrorMessage(PutEventGuestsCommandValidator.CalendarEventIsInPast);
        result.Errors.Count.Should().Be(1);
    }


    [TestCase("", "title")]
    [TestCase("name", "")]
    [TestCase("", "")]
    public async Task Validate_GuestNameAndJobTitleIncomplete(string guestName, string guestTitle)
    {
        Mock<IMembersReadRepository> membersReadRepository = new();
        var command = GetCommand();

        var member = new Member
        {
            UserType = UserType.Admin,
            Status = MembershipStatusType.Live.ToString(),
            IsRegionalChair = true
        };

        var guestList = command.Guests.ToList();
        guestList.Add(new EventGuestModel(guestName, guestTitle));
        command.Guests = guestList;

        membersReadRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

        PutEventGuestsCommandValidator sut = new(membersReadRepository.Object);

        var result = await sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Guests)
            .WithErrorMessage(PutEventGuestsCommandValidator.GuestNamesAndJobTitlesMustBePresent);
        result.Errors.Count.Should().Be(1);
    }


    public static PutEventGuestsCommand GetCommand()
    {
        return new PutEventGuestsCommand()
        {
            AdminMemberId = Guid.NewGuid(),
            CalendarEventId = Guid.NewGuid(),
            CalendarEvent = new CalendarEvent { IsActive = true, StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(1).AddHours(1) },
            Guests = new List<EventGuestModel>
            {
                new EventGuestModel("name", "title")
            }
        };
    }
}

