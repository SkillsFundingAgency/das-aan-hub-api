namespace SFA.DAS.AANHub.Api.Models
{
    public class UpdateMemberNotificationLocationsApiRequest
    {
        public List<Location> Locations { get; set; } = [];

        public class Location
        {
            public string Name { get; set; } = string.Empty;
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
