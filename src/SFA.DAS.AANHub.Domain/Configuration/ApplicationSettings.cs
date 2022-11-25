using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Domain.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ApplicationSettings
    {
        public string? DbConnectionString { get; set; }
    }
}
