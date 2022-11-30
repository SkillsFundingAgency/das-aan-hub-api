using AutoFixture.Xunit2;
using FluentAssertions;
using SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Queries
{
    public class WhenRequestingCalendarsForUser
    {
        [Theory, AutoMoqData]
        public async Task ThenAllCalendarsForUserAreReturned(
            GetCalendarsForUserQuery query,
            [Frozen(Matching.ImplementedInterfaces)] AanDataContext context,
            GetCalendarsForUserQueryHandler handler)
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

            var memberPermissions = new List<MemberPermission>{
                new()
                {
                    IsActive = true,
                    MemberId = memberGuid,
                    PermissionId = 2345
                }};

            context.Calendars.AddRange(calendar);
            context.CalendarPermissions.AddRange(calendarPermission);
            context.MemberPermissions.AddRange(memberPermissions);
            await context.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);


            result?.Should().NotBeNull();

            result?.Calendars?.ToList().Count.Should().Be(1);
            result?.Permissions?.ToList()[0].Should().Be(2345);


        }
    }
}
