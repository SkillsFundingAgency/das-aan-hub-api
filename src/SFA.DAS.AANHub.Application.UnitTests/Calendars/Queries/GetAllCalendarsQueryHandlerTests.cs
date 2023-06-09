using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Calendars.Queries;

public class GetAllCalendarsQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]

    public async Task Handle_ReturnAllCalendars(
        GetCalendarsQuery query,
        [Frozen] Mock<ICalendarsReadRepository> calendarsReadRepositoryMock,
        GetCalendarsQueryHandler sut,
        List<Calendar> calendars,
        CancellationToken cancellationToken)
    {
        calendarsReadRepositoryMock.Setup(r => r.GetAllCalendars(cancellationToken)).ReturnsAsync(calendars);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Calendars.Count().Should().Be(calendars.Count);
    }
}
