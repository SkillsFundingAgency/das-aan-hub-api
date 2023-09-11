using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class OptingOutEmailTemplate
{
    public OptingOutEmailTemplate(string firstName, string lastName)
    {
        Contact = $"{firstName} {lastName}";
    }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }
}
