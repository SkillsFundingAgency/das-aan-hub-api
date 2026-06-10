namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryResult
{
    public bool? ReceiveNotifications { get; set; }
    public List<NotificationEventType> EventTypes = [];
    public List<Location> Locations { get; set; } = [];

    public class NotificationEventType
    {
        public string EventType { get; set; } = null!;
        public bool ReceiveNotifications { get; set; }
    }

    public class Location
    {
        public string Name { get; set; } = null!;
        public int Radius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }


}
