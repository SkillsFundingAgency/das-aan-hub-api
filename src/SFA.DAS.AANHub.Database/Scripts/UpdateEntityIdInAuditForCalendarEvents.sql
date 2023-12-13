update audit 
	set EntityId = JSON_VALUE(Before,'$.Id')
		where resource = 'CalendarEvent' 
		and EntityId is null
		and JSON_VALUE(Before,'$.Id') is not null

update audit 
	set EntityId = JSON_VALUE(After,'$.Id')
		where resource = 'CalendarEvent' 
		and EntityId is null
		and JSON_VALUE(After,'$.Id') is not null
