﻿namespace SFA.DAS.AANHub.Domain.Entities;

public class Attendance
{
    public Guid Id { get; set; }    
    public Guid CalendarEventId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime? Added { get; set; }
    public bool IsActive { get; set; }
    public bool Attended { get; set; }
    public int? OverallRating { get; set; }
    public DateTime? FeedbackCompletedDate { get; set; }

    public CalendarEvent CalendarEvent { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
