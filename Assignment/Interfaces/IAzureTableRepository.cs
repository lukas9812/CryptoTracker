using Assignment.Models;
using Assignment.Storage;

namespace Assignment.Interfaces;

public interface IAzureTableRepository
{
    Task UpsertCryptoData(CryptoModel model);
    
    Task<List<CryptoEntityDo>> GetByTimeStamp(int minusMinutes);

    Task<CryptoEntityDo> GetLastRecordByPartitionKey(string partitionKey);
}