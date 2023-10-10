using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutGuestSpeakers;
using SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutEventGuests;

public class PutEventGuestsCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_PutEventGuests(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IEventGuestsWriteRepository> eventGuestsWriteRepository,
        [Frozen] PutEventGuestsCommandHandler sut)
    {
        var command = new PutEventGuestsCommand();
        var guests = command.Guests.Select(x => new EventGuest { GuestName = x.GuestName, GuestJobTitle = x.GuestJobTitle, CalendarEventId = command.CalendarEventId }).ToList();

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            eventGuestsWriteRepository.Verify(e => e.DeleteAll(command.CalendarEventId), Times.Once);
            eventGuestsWriteRepository.Verify(e => e.CreateGuests(guests), Times.Once);
            auditWriteRepository.Verify(a => a.Create(It.IsAny<Audit>()), Times.Once);
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}