﻿using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class OnboardingEmailTemplate
{
    public OnboardingEmailTemplate(string firstName, string lastName, string region)
    {
        Contact = $"{firstName} {lastName}";
        Region = region;
    }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }
    [JsonPropertyName("region")]
    public string Region { get; set; }
}
