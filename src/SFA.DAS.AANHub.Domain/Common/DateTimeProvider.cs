using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Domain.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
