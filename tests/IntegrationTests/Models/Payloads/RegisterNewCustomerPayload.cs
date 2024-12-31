using System;
using System.Text.Json.Serialization;

namespace IntegrationTests.Models.Payloads;

public class RegisterNewCustomerPayload
{
    [JsonPropertyName("first-name")]
    public string FirstName { get; init; }
    
    [JsonPropertyName("last-name")]
    public string LastName { get; init; }
    
    [JsonPropertyName("birth-date")]
    public string BirthDate { get; init; }
    
    [JsonPropertyName("email")]
    public string Email { get; init; }
}
