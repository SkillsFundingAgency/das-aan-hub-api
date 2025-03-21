﻿using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationSettings
{
    public class UpdateMemberNotificationSettingsCommandHandler(
        IAanDataContext aanDataContext,
        IMembersWriteRepository membersWriteRepository)
        : IRequestHandler<UpdateMemberNotificationSettingsCommand>
    {
        public async Task Handle(UpdateMemberNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var member = await membersWriteRepository.Get(request.MemberId);

            if (member == null)
            {
                throw new ArgumentException($"Member id {request.MemberId} not found");
            }

            if (!request.ReceiveNotifications)
            {
                request.EventTypes.Clear();
                request.Locations.Clear();
            }
            else if (request.EventTypes.Count == 1 && request.EventTypes.First().EventType == "Online")
            {
                request.Locations.Clear();
            }

            member.ReceiveNotifications = request.ReceiveNotifications;
            UpdateEventTypes(member, request);
            UpdateLocations(member, request);

            await aanDataContext.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEventTypes(Member member, UpdateMemberNotificationSettingsCommand request)
        {
            var existingEventTypes = member.MemberNotificationEventFormats;

            // If "All" is selected, replace it with all possible event types
            if (request.EventTypes.Any(e => e.EventType == "All"))
            {
                request.EventTypes =
                [
                    new() { EventType = "InPerson", ReceiveNotifications = true},
                    new() { EventType = "Online", ReceiveNotifications = true },
                    new() { EventType = "Hybrid", ReceiveNotifications = true }
                ];
            }

            // Remove event types that are not in the request
            var eventTypesToRemove = existingEventTypes
                .Where(existingEventType => !request.EventTypes.Any(requestEventType =>
                    requestEventType.EventType == existingEventType.EventFormat))
                .ToList();

            foreach (var eventType in eventTypesToRemove)
            {
                member.MemberNotificationEventFormats.Remove(eventType);
            }

            //Modify existing event types in the request that are set to false
            member.MemberNotificationEventFormats.ForEach(e =>
            {
                if (request.EventTypes.Any(r => r.EventType == e.EventFormat))
                {
                    e.ReceiveNotifications = true;
                }
            });

            // Add event types that are in the request but not already in the existing event types
            var eventTypesToAdd = request.EventTypes
                .Where(requestEventType => existingEventTypes.All(existingEventType => requestEventType.EventType != existingEventType.EventFormat))
                .ToList();

            foreach (var eventType in eventTypesToAdd)
            {
                member.MemberNotificationEventFormats.Add(new MemberNotificationEventFormat
                {
                    MemberId = member.Id,
                    EventFormat = eventType.EventType,
                    ReceiveNotifications = eventType.ReceiveNotifications
                });
            }
        }

        private void UpdateLocations(Member member, UpdateMemberNotificationSettingsCommand request)
        {
            request.Locations =
                request.Locations.DistinctBy(x => HashCode.Combine(x.Name, x.Radius, x.Latitude, x.Longitude)).ToList();

            var existingLocations = member.MemberNotificationLocations;

            // Remove locations that are not in the request
            var locationsToRemove = existingLocations
                .Where(existingLocation => !request.Locations.Any(requestLocation =>
                    requestLocation.Name == existingLocation.Name &&
                    requestLocation.Radius == existingLocation.Radius &&
                    requestLocation.Latitude == existingLocation.Latitude &&
                    requestLocation.Longitude == existingLocation.Longitude))
                .ToList();

            foreach (var location in locationsToRemove)
            {
                member.MemberNotificationLocations.Remove(location);
            }

            // Add locations that are in the request but not already in the existing locations
            var locationsToAdd = request.Locations
                .Where(requestLocation => !existingLocations.Any(existingLocation =>
                    requestLocation.Name == existingLocation.Name &&
                    requestLocation.Radius == existingLocation.Radius &&
                    requestLocation.Latitude == existingLocation.Latitude &&
                    requestLocation.Longitude == existingLocation.Longitude))
                .ToList();

            foreach (var location in locationsToAdd)
            {
                member.MemberNotificationLocations.Add(new MemberNotificationLocation
                {
                    MemberId = member.Id,
                    Name = location.Name,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Radius = location.Radius
                });
            }
        }
    }
}
