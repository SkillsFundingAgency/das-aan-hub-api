
using SFA.DAS.AAN.Application.Commands.CalendarEventCommands;
using SFA.DAS.AAN.Domain.Entities;


namespace SFA.DAS.AAN.Application.Queries.GetCalendarEvents
{
    public class GetCalendarEventsResultItem : CalendarEventCommandBase
    {
        public Guid calendareventid { get; set; }
        public DateTime created { get; set; }

        public GetCalendarEventsResultItem(CalendarEvent e)
        {
            this.calendareventid = e.Id;
            this.created = e.Created;
            this.start = e.Start;
            this.end = e.End;
            this.description = e.Description;
            this.regionid = e.RegionId;
            this.location = e.Location;
            this.postcode = e.Postcode;
            this.eventlink = e.EventLink;
            this.contact = e.Contact;
            this.email = e.ContactEmail;
        }
    }
}
