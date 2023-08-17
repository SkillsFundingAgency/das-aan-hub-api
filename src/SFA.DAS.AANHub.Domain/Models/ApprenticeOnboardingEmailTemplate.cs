using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class ApprenticeOnboardingEmailTemplate
{
    public ApprenticeOnboardingEmailTemplate(string firstName, string lastName, string region, string link)
    {
        Contact = $"{firstName} {lastName}";
        Region = region;
        Link = link;
    }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }
    [JsonPropertyName("region")]
    public string Region { get; set; }
    [JsonPropertyName("link")]
    public string Link { get; set; }
}
