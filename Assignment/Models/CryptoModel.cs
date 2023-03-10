using System.Text.Json.Serialization;

namespace Assignment.Models;

public class CryptoModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price_usd")] 
    public double Price { get; set; } = default;
    
    [JsonIgnore]
    public DateTimeOffset CurrentDateTime { get; set; } = DateTimeOffset.Now;
}