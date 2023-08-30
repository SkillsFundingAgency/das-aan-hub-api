using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class MemberToMemberContactEmailTemplate
{
    public MemberToMemberContactEmailTemplate(string recieverFullName, string requestorFullName, string requestorFirstName, string requestorEmailAddress)
    {
        Contact = recieverFullName;
        RequesterFullName = requestorFullName;
        RequesterFirstName = requestorFirstName;
        RequesterEmailAddress = requestorEmailAddress;
    }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }

    [JsonPropertyName("requesterfullname")]
    public string RequesterFullName { get; set; }

    [JsonPropertyName("requesterfirstname")]
    public string RequesterFirstName { get; set; }

    [JsonPropertyName("requesteremail")]
    public string RequesterEmailAddress { get; set; }
}
