
  update A Set A.EntityId = CE.Id
	from Attendance Att 
	inner join CalendarEvent CE on Att.CalendarEventId = CE.Id
	inner join Audit A on A.EntityId = Att.Id 
	where A.[Resource] = 'Attendance'