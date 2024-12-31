using System;
using System.Text.Json.Serialization;

namespace IntegrationTests.Models.ViewModels;

public class CustomerViewModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("first-name")]
    public string FirstName { get; init; }
    
    [JsonPropertyName("last-name")]
    public string LastName { get; init; }
    
    [JsonPropertyName("birth-date")]
    public string BirthDate { get; init; }
    
    [JsonPropertyName("email")]
    public string Email { get; init; }
}
