using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries
{
    public class GetStagedApprenticeResult
    {
        public long Uln { get; set; }
        public long ApprenticeshipId { get; set; }
        public string? EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long TrainingProviderId { get; set; }
        public string? TrainingProviderName { get; set; }
        public string? TrainingCode { get; set; }
        public string? StandardUId { get; set; }

        public static implicit operator GetStagedApprenticeResult?(StagedApprentice stagedApprentice)
        {
            if (stagedApprentice == null)
                return null;

            return new GetStagedApprenticeResult
            {
                Uln = stagedApprentice.Uln,
                ApprenticeshipId = stagedApprentice.ApprenticeshipId,
                EmployerName = stagedApprentice.EmployerName,
                StartDate = stagedApprentice.StartDate,
                EndDate = stagedApprentice.EndDate,
                TrainingProviderId = stagedApprentice.TrainingProviderId,
                TrainingProviderName = stagedApprentice.TrainingProviderName,
                TrainingCode = stagedApprentice.TrainingCode,
                StandardUId = stagedApprentice.StandardUId
            };
        }
    }
}
