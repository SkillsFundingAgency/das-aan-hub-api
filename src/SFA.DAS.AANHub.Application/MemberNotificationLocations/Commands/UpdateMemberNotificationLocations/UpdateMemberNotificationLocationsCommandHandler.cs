using MediatR;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationLocations;

public class UpdateMemberNotificationLocationsCommandHandler : IRequestHandler<UpdateMemberNotificationLocationsCommand>
{
    private readonly IMemberNotificationLocationWriteRepository _memberNotificationLocationWriteRepository;
    private readonly IMemberNotificationLocationReadRepository _memberNotificationLocationReadRepository;
    private readonly IAanDataContext _aanDataContext;

    public UpdateMemberNotificationLocationsCommandHandler(
        IMemberNotificationLocationWriteRepository memberNotificationLocationWriteRepository,
        IMemberNotificationLocationReadRepository memberNotificationLocationReadRepository,
        IAanDataContext aanDataContext)
    {
        _memberNotificationLocationWriteRepository = memberNotificationLocationWriteRepository;
        _memberNotificationLocationReadRepository = memberNotificationLocationReadRepository;
        _aanDataContext = aanDataContext;
    }

    public async Task Handle(UpdateMemberNotificationLocationsCommand request, CancellationToken cancellationToken)
    {
        var existingLocations = (await _memberNotificationLocationReadRepository
            .GetMemberNotificationLocationsByMember(request.MemberId, cancellationToken)).ToList() ?? new List<MemberNotificationLocation>();

        // Remove locations that are not in the request
        var locationsToRemove = existingLocations
            .Where(existingLocation => !request.Locations.Any(requestLocation =>
                requestLocation.Name == existingLocation.Name &&
                requestLocation.Radius == existingLocation.Radius &&
                requestLocation.Latitude == existingLocation.Latitude &&
                requestLocation.Longitude == existingLocation.Longitude))
            .ToList();

        _memberNotificationLocationWriteRepository.DeleteMemberNotificationLocations(locationsToRemove, cancellationToken);

        foreach (var location in locationsToRemove)
        {
            existingLocations.Remove(location);
        }

        // Add locations that are in the request but not already in the existing locations
        var locationsToAdd = request.Locations
            .Where(requestLocation => !existingLocations.Any(existingLocation =>
                requestLocation.Name == existingLocation.Name &&
                requestLocation.Radius == existingLocation.Radius &&
                requestLocation.Latitude == existingLocation.Latitude &&
                requestLocation.Longitude == existingLocation.Longitude))
            .ToList();

        existingLocations.AddRange(locationsToAdd.Select(x => new MemberNotificationLocation
        {
            MemberId = request.MemberId,
            Name = x.Name,
            Radius = x.Radius,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
        }));

        // Update member's locations
        _memberNotificationLocationWriteRepository.UpdateMemberNotificationLocations(existingLocations, cancellationToken);

        await _aanDataContext.SaveChangesAsync(cancellationToken);
    }
}