using System;
using System.Text.Json.Serialization;
using IntegrationTests.Models.Responses.Enums;

namespace IntegrationTests.Models.Responses.Base;

public class ResponseMessage
{
    [JsonPropertyName("type")]
    public ResponseMessageType Type { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
