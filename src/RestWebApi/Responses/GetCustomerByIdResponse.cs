using System;
using System.Text.Json.Serialization;
using MCIO.Mentorship.RestWebApi.ViewModels;

namespace MCIO.Mentorship.RestWebApi.Responses;

public class GetCustomerByIdResponse
{
    [JsonPropertyName("data")]
    public CustomerViewModel Data { get; set; }
}
