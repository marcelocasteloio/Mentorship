using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntegrationTests.Models.Responses.Base;

public abstract class ReponseBase<TData>
{
    [JsonPropertyName("data")]
    public TData Data { get; init; }

    [JsonPropertyName("messages")]
    public IEnumerable<ResponseMessage>? MessageCollection { get; init; }
}
