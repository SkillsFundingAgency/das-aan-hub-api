using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.AANHub.Application.UnitTests.MemberNotificationLocations.Queries
{
    [TestFixture]
    public class GetMemberNotificationSettingsQueryHandlerTests
    {
        [Test, CustomAutoData]
        public async Task Handle_Returns_Correct_Result_When_Member_Found(
            GetMemberNotificationSettingsQuery query,
            Member member,
            [Frozen] Mock<IMembersReadRepository> mockMemberRepository,
            GetMemberNotificationSettingsQueryHandler handler)
        {
            // Arrange
            mockMemberRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync(member);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ReceiveNotifications.Should().Be(member.ReceiveNotifications);
            result.EventTypes.Should().BeEquivalentTo(member.MemberNotificationEventFormats.Select(x => new GetMemberNotificationSettingsQueryResult.NotificationEventType
            {
                ReceiveNotifications = x.ReceiveNotifications,
                EventType = x.EventFormat,
                Ordering = x.Ordering
            }));
            result.Locations.Should().BeEquivalentTo(member.MemberNotificationLocations.Select(x => new GetMemberNotificationSettingsQueryResult.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }));
        }

        [Test, MoqAutoData]
        public void Handle_Throws_Exception_When_Member_Not_Found(
            GetMemberNotificationSettingsQuery query,
            [Frozen] Mock<IMembersReadRepository> mockMemberRepository,
            GetMemberNotificationSettingsQueryHandler handler)
        {
            // Arrange
            mockMemberRepository.Setup(x => x.GetMember(It.IsAny<Guid>())).ReturnsAsync((Member)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
