using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationSettings;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberNotificationLocations.Commands
{
    [TestFixture]
    public class UpdateMemberNotificationSettingsCommandHandlerTests
    {
        [Test, CustomAutoData]
        public async Task Handle_Removes_Deleted_Locations(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            existingMember.MemberNotificationLocations.Add(new MemberNotificationLocation
            {
                Name = "Location to be removed",
                Radius = 50,
                Latitude = 0,
                Longitude = 0
            });
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);
            command.Locations =
                command.Locations.Take(1).ToList(); // Ensure command locations don't match all existing locations

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationLocations.Should().NotContain(loc => loc.Name == "Location to be removed");
        }

        [Test, CustomAutoData]
        public async Task Handle_Adds_New_Locations(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            existingMember.MemberNotificationLocations.Clear(); // Ensure no existing locations
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationLocations.Should()
                .BeEquivalentTo(command.Locations, options => options.ExcludingMissingMembers());
        }

        [Test, CustomAutoData]
        public async Task Handle_Ingores_Duplicates_When_Adding_New_Locations(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            command.Locations.Clear();
            command.Locations.Add(new UpdateMemberNotificationSettingsCommand.Location { Name = "Test location", Radius = 10, Latitude = 1, Longitude = 2 });
            command.Locations.Add(new UpdateMemberNotificationSettingsCommand.Location { Name = "Test location", Radius = 10, Latitude = 1, Longitude = 2 });
            existingMember.MemberNotificationLocations.Clear(); // Ensure no existing locations
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationLocations.Should().HaveCount(1);
        }

        [Test, CustomAutoData]
        public async Task Handle_Updates_Existing_Locations(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            var existingLocation = existingMember.MemberNotificationLocations.First();
            command.Locations = new List<UpdateMemberNotificationSettingsCommand.Location>
            {
                new UpdateMemberNotificationSettingsCommand.Location
                {
                    Name = existingLocation.Name,
                    Radius = existingLocation.Radius,
                    Latitude = existingLocation.Latitude,
                    Longitude = existingLocation.Longitude
                }
            };
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationLocations.Should().ContainSingle(loc =>
                loc.Name == existingLocation.Name &&
                loc.Radius == existingLocation.Radius &&
                loc.Latitude == existingLocation.Latitude &&
                loc.Longitude == existingLocation.Longitude);
        }

        [Test, CustomAutoData]
        public async Task Handle_Removes_Deleted_EventTypes(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            existingMember.MemberNotificationEventFormats.Add(new MemberNotificationEventFormat
            {
                EventFormat = "Event to be removed"
            });
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);
            command.EventTypes =
                command.EventTypes.Take(1).ToList(); // Ensure command event types don't match all existing event types

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationEventFormats.Should()
                .NotContain(evt => evt.EventFormat == "Event to be removed");
        }

        [Test, CustomAutoData]
        public async Task Handle_Adds_New_EventTypes(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            existingMember.MemberNotificationEventFormats.Clear(); // Ensure no existing event types
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationEventFormats.Should()
                .BeEquivalentTo(command.EventTypes, options => options.ExcludingMissingMembers());
        }

        [Test, CustomAutoData]
        public async Task Handle_Updates_Existing_EventTypes(
            UpdateMemberNotificationSettingsCommand command,
            [Frozen] Mock<IMembersWriteRepository> mockMembersWriteRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            Member existingMember,
            UpdateMemberNotificationSettingsCommandHandler handler)
        {
            // Arrange
            var existingEventType = existingMember.MemberNotificationEventFormats.First();
            command.EventTypes = new List<UpdateMemberNotificationSettingsCommand.NotificationEventType>
            {
                new UpdateMemberNotificationSettingsCommand.NotificationEventType()
                {
                    EventType = existingEventType.EventFormat
                }
            };
            mockMembersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(existingMember);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            existingMember.MemberNotificationEventFormats.Should().ContainSingle(evt =>
                evt.EventFormat == existingEventType.EventFormat);
        }
    }
}