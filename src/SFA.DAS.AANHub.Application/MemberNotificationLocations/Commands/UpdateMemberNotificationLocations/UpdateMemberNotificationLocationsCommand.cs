using MediatR;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationLocations
{
    public class UpdateMemberNotificationLocationsCommand : IRequest
    {
        public Guid MemberId { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();

        public class Location
        {
            public string Name { get; set; } = string.Empty;
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
