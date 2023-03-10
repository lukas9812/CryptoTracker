namespace Assignment.Models;

public record MyOptions
{
    public required string CryptoCurrencyName { get; init; } = string.Empty;

    public required string ApiKeyToCoinApi { get; init; } = string.Empty;

    public required int GetByTimeStamp { get; init; } = default!;

    public required string PartitionKey { get; init; } = string.Empty;
}