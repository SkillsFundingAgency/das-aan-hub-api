
-- CSP-1023, we are dropping 'Other' Id 7 from the Calendar table.  The following will clear down any eventGuest, attendance 
-- or CalendarEvents linked to this script can be removed once all environments have this change applied (including Demo and MO)
delete from EventGuest where CalendarEventId in (select id from CalendarEvent where CalendarId=7)
delete from Attendance where CalendarEventId in (select id from CalendarEvent where CalendarId=7)
delete from [CalendarEvent] where calendarId=7