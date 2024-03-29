﻿namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryResult
{
    public DateTime? LastSignedUpDate { get; set; }

    public EventsModel EventsAttended { get; set; } = null!;

    public EventsModel EventsPlanned { get; set; } = null!;
}
