using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models;

/// <summary>
/// Represents a bulk transaction to be sent to the MapLibre map.
/// This can be used to send multiple layers and sources to the map in a websocket message.
/// This can be useful to reduce the roundtrip time when adding multiple layers and sources to the map.
/// </summary>
public class BulkTransaction
{
    public List<Transaction> Transactions { get; } = [];

    public void Add(string eventName, params object?[]? data)
    {
        Transactions.Add(new Transaction
        {
            Event = eventName,
            Data = data
        });
    }
}

public class Transaction
{
    [JsonPropertyName("event")]
    public required string Event { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object?[]? Data { get; set; }
}
