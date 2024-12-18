using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationLocations;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberNotificationLocations.Commands
{
    [TestFixture]
    public class UpdateMemberNotificationLocationsCommandHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_Removes_Deleted_Locations(
            UpdateMemberNotificationLocationsCommand command,
            [Frozen] Mock<IMemberNotificationLocationWriteRepository> mockWriteRepository,
            [Frozen] Mock<IMemberNotificationLocationReadRepository> mockReadRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            List<MemberNotificationLocation> existingLocations,
            UpdateMemberNotificationLocationsCommandHandler handler)
        {
            // Arrange
            existingLocations.Add(new MemberNotificationLocation
            {
                Name = "Location to be removed",
                Radius = 50,
                Latitude = 0,
                Longitude = 0
            });
            mockReadRepository.Setup(x => x.GetMemberNotificationLocationsByMember(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingLocations);
            command.Locations = command.Locations.Take(1).ToList(); // Ensure command locations don't match all existing locations

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockWriteRepository.Verify(x => x.DeleteMemberNotificationLocations(It.Is<List<MemberNotificationLocation>>(list => list.Any(x => x.Name == "Location to be removed")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_Adds_New_Locations(
            UpdateMemberNotificationLocationsCommand command,
            [Frozen] Mock<IMemberNotificationLocationWriteRepository> mockWriteRepository,
            [Frozen] Mock<IMemberNotificationLocationReadRepository> mockReadRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            List<MemberNotificationLocation> existingLocations,
            UpdateMemberNotificationLocationsCommandHandler handler)
        {
            // Arrange
            existingLocations.Clear(); // Ensure no existing locations
            mockReadRepository.Setup(x => x.GetMemberNotificationLocationsByMember(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingLocations);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockWriteRepository.Verify(x => x.UpdateMemberNotificationLocations(It.Is<List<MemberNotificationLocation>>(list =>
                list.Count == command.Locations.Count &&
                list.All(loc => command.Locations.Any(reqLoc =>
                    reqLoc.Name == loc.Name &&
                    reqLoc.Radius == loc.Radius &&
                    reqLoc.Latitude == loc.Latitude &&
                    reqLoc.Longitude == loc.Longitude))), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_Updates_Existing_Locations(
            UpdateMemberNotificationLocationsCommand command,
            [Frozen] Mock<IMemberNotificationLocationWriteRepository> mockWriteRepository,
            [Frozen] Mock<IMemberNotificationLocationReadRepository> mockReadRepository,
            [Frozen] Mock<IAanDataContext> mockAanDataContext,
            List<MemberNotificationLocation> existingLocations,
            UpdateMemberNotificationLocationsCommandHandler handler)
        {
            // Arrange
            var existingLocation = existingLocations.First();
            command.Locations = new List<UpdateMemberNotificationLocationsCommand.Location>
            {
                new UpdateMemberNotificationLocationsCommand.Location
                {
                    Name = existingLocation.Name,
                    Radius = existingLocation.Radius,
                    Latitude = existingLocation.Latitude,
                    Longitude = existingLocation.Longitude
                }
            };
            mockReadRepository.Setup(x => x.GetMemberNotificationLocationsByMember(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingLocations);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockWriteRepository.Verify(x => x.UpdateMemberNotificationLocations(It.Is<List<MemberNotificationLocation>>(list =>
                list.Contains(existingLocation) &&
                list.Count == 1), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
