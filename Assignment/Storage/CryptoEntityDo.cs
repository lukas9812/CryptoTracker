﻿using Azure;
using Azure.Data.Tables;

namespace Assignment.Storage;

public class CryptoEntityDo : ITableEntity
{
    public string Name { get; init; } = string.Empty;
    
    public double Price { get; init; }
    
    public DateTimeOffset CurrentDateTime { get; init; }

    public string PartitionKey { get; set; } = string.Empty;
    
    public string RowKey { get; set; } = string.Empty;
    
    
    public DateTimeOffset? Timestamp { get; set; }
    

    public ETag ETag { get; set; }
}