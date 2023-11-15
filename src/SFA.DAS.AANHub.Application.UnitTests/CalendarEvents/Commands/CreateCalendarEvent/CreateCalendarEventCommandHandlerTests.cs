using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandHandlerTests
{
    Mock<ICalendarEventsWriteRepository> _calendarEventRepoMock = null!;
    Mock<IAuditWriteRepository> _auditWriteRepoMock = null!;
    Mock<IAanDataContext> _dataContextMock = null!;
    CreateCalendarEventCommandHandler _sut = null!;
    CreateCalendarEventCommand _command = null!;
    CancellationToken _cancellationToken;

    [SetUp]
    public async Task Initialise()
    {
        _calendarEventRepoMock = new();
        _auditWriteRepoMock = new();
        _dataContextMock = new();
        _sut = new(_calendarEventRepoMock.Object, _auditWriteRepoMock.Object, _dataContextMock.Object);
        Fixture fixture = new();
        _command = fixture.Create<CreateCalendarEventCommand>();
        _cancellationToken = new();

        //Action
        await _sut.Handle(_command, _cancellationToken);
    }

    [Test]
    public void ThenAddsCalendarEvent() =>
        _calendarEventRepoMock.Verify(r => r.Create(It.IsAny<CalendarEvent>()));

    [Test]
    public void ThenAddsAuditRecord() =>
        _auditWriteRepoMock.Verify(a => a.Create(It.Is<Audit>(a =>
            a.Action == "Create" &&
            a.Before == null &&
            a.After != null &&
            a.ActionedBy == _command.AdminMemberId &&
            a.Resource == nameof(CalendarEvent))));

    [Test]
    public void ThenInvokesDataContextSaveChanges() =>
        _dataContextMock.Verify(c => c.SaveChangesAsync(_cancellationToken));
}
