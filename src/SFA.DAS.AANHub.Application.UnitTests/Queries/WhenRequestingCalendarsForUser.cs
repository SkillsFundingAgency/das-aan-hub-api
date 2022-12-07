using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Queries
{
    public class WhenRequestingCalendarsForUser
    {
        private readonly Mock<ICalendarsReadRepository> _calendarsReadRepository;
        private readonly Mock<ICalendarsPermissionsReadRepository> _calendarsPermissionsReadRepository;
        private readonly Mock<IMembersPermissionsReadRepository> _membersPermissionsReadRepository;
        private readonly GetCalendarsForUserQueryHandler _handler;

        public WhenRequestingCalendarsForUser()
        {
            _calendarsReadRepository = new Mock<ICalendarsReadRepository>();
            _calendarsPermissionsReadRepository = new Mock<ICalendarsPermissionsReadRepository>();
            _membersPermissionsReadRepository = new Mock<IMembersPermissionsReadRepository>();
            _handler = new GetCalendarsForUserQueryHandler(_calendarsReadRepository.Object, _calendarsPermissionsReadRepository.Object, _membersPermissionsReadRepository.Object);
        }

        [Test, AutoMoqData]
        public async Task ThenAllCalendarsForUserAreReturned(
            GetCalendarsForUserQuery query)
        {
            var memberGuid = Guid.NewGuid();

            query.MemberId = memberGuid;
            var calendar = new List<Calendar>{new()
            {
                CalendarName = "TestCalendar",
                Description = "Description",
                EffectiveFrom = DateTime.Now,
                EffectiveTo = DateTime.Now,
                Id = 1234,
                IsActive = true
            }};

            var calendarPermission = new List<CalendarPermission>{new()
            {
                CalendarId = 1234,
                Create = true,
                Delete = true,
                PermissionId = 2345,
                Update = true,
                View = true
            }};

            var memberPermissions = new List<long> { 2345 };

            _calendarsReadRepository.Setup(m => m.GetAllCalendars()).ReturnsAsync(calendar);
            _calendarsPermissionsReadRepository.Setup(m => m.GetAllCalendarsPermissionsByPermissionIds(It.IsAny<List<long>>())).ReturnsAsync(calendarPermission);
            _membersPermissionsReadRepository.Setup(m => m.GetAllMemberPermissionsForUser(It.IsAny<Guid>())).ReturnsAsync(memberPermissions);

            var result = await _handler.Handle(query, CancellationToken.None);

            result?.Should().NotBeNull();

            result?.Calendars?.ToList().Count.Should().Be(1);
            result?.Permissions?.ToList()[0].Should().Be(2345);

        }
    }
}
