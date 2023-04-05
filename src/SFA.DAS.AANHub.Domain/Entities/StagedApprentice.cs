namespace SFA.DAS.AANHub.Domain.Entities
{
    public class StagedApprentice
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public long Uln { get; set; }
        public long ApprenticeshipId { get; set; }
        public string?EmployerName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; } = string.Empty;
        public string TrainingCode { get; set; } = null!;
        public string TrainingCourseOption { get; set; } = string.Empty;
        public string StandardUId { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}