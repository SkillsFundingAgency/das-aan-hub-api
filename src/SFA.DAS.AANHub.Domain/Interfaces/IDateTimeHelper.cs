using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Interfaces
{

    public interface IDateTimeHelper
    {
        DateTime Now { get; }
    }
    [ExcludeFromCodeCoverage]
    public class UtcTimeProvider : IDateTimeHelper
    {
        public DateTime Now => DateTime.UtcNow;
    }
    [ExcludeFromCodeCoverage]
    public class SpecifiedTimeProvider : IDateTimeHelper
    {
        public DateTime Now { get; set; }

        public SpecifiedTimeProvider(DateTime time)
        {
            Now = time;
        }

        public void Advance(TimeSpan timeSpan)
        {
            Now = Now.Add(timeSpan);
        }
    }
}
