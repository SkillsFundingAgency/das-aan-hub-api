using SFA.DAS.AANHub.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.AANHub.Api.Models
{
    public class UpdateMemberNotificationSettingsApiRequest
    {
        public bool ReceiveNotifications { get; set; }
        public List<NotificationEventType> EventTypes { get; set; } = [];
        public List<Location> Locations { get; set; } = [];

        public class NotificationEventType
        {
            [Required]
            [EnumDataType(typeof(EventFormat), ErrorMessage = "Invalid EventType. Allowed values: InPerson, Online, Hybrid.")]
            public string EventType { get; set; }
            public bool ReceiveNotifications { get; set; }
        }

        public class Location
        {
            [RegularExpression(@"^[a-zA-Z0-9\s,']+$", ErrorMessage = "Name contains invalid characters.")]
            public string Name { get; set; }
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
