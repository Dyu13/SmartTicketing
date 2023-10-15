using System.Text.Json.Serialization;

namespace SmartTicketing.Common;

public class MessagePayload
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("connectionId")]
    public string ConnectionId { get; set; }
}
