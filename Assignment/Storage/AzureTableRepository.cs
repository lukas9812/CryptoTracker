using System.Globalization;
using Assignment.Interfaces;
using Assignment.Models;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Assignment.Storage;

public class AzureTableRepository : IAzureTableRepository
{
    private readonly TableClient _azureTable;
    private readonly ILogger<AzureTableRepository> _logger;


    public AzureTableRepository(TableServiceClient tableClient, ILogger<AzureTableRepository> logger)
    {
        _logger = logger;
        _azureTable = tableClient.GetTableClient("CryptoOutputs");
    }

    private async Task<TableClient> GetAzureTable()
    {
        await _azureTable.CreateIfNotExistsAsync();
        return _azureTable;
    }

    public async Task UpsertCryptoData(CryptoModel model)
    {
        var azureTable = await GetAzureTable();

        var azureData = new CryptoEntityDo
        {
            Name = model.Name,
            CurrentDateTime = model.CurrentDateTime,
            Price = model.Price,
            PartitionKey = model.Name + "_Coin",
            RowKey = Guid.NewGuid().ToString(),
        };
        
        _logger.LogInformation("Posting a new {partitionKey} value at {time}", azureData.PartitionKey, DateTime.Now);
        await azureTable.UpsertEntityAsync(azureData);
    }

    public async Task<List<CryptoEntityDo>> GetByTimeStamp(int minusMinutes)
    {
        _logger.LogInformation("Getting Azure table record by a time-stamp property");
        var azureTable = await GetAzureTable();
        var tmp = azureTable
            .Query<CryptoEntityDo>(entity => entity.Timestamp > DateTimeOffset.Now.AddMinutes(-minusMinutes))
            .ToList();
        return await Task.FromResult(tmp);
    }

    public async Task<CryptoEntityDo> GetLastRecordByPartitionKey(string partitionKey)
    {
        _logger.LogInformation("Getting last record Azure table by partition key property.");
        var azureTable = await GetAzureTable();

        var lastEntity = azureTable.Query<CryptoEntityDo>(entity => entity.PartitionKey == partitionKey)
            .MaxBy(x => x.Timestamp);




        return lastEntity ?? new CryptoEntityDo();
    }
    
}