using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Assignment;

public class PostCryptoValues
{
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<PostCryptoValues> _logger;
    private readonly IAzureTableRepository _tableRepository;

    public PostCryptoValues(IHttpClientService httpClient, 
        ILogger<PostCryptoValues> logger,
        IAzureTableRepository tableRepository)
    {
        _httpClient = httpClient;
        _logger = logger;
        _tableRepository = tableRepository;
    }

    [Function("PostCryptoValues")]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]MyInfo myTimer, FunctionContext context)
    {
        var logger = context.GetLogger("GetCryptoValues");
        logger.LogInformation("Timer trigger function executed at: {Now}", DateTime.Now);

        var result = await _httpClient.GetFromJsonAsync();
        
        var cryptoModel = (JsonSerializer.Deserialize<List<CryptoModel>>(result) ??
                           throw new NullReferenceException("Cannot extract data from CoinAPI"))
            .FirstOrDefault()!;
        
        logger.LogInformation("Upsert extracted data to the AzureTable Storage");
        await _tableRepository.UpsertCryptoData(cryptoModel);
        
        _logger.LogInformation("Next timer schedule at: {NextSchedule}", myTimer.ScheduleStatus.Next);
    }
}












public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}