using System.Net;
using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assignment;

public class GetOldValuesByMinutes
{
    private readonly IAzureTableRepository _tableRepository;
    private readonly MyOptions _settings;

    public GetOldValuesByMinutes(IAzureTableRepository tableRepository, IOptions<MyOptions> settings)
    {
        _tableRepository = tableRepository;
        _settings = settings.Value;
    }

    [Function("GetOldValuesByMinutes")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("GetCurrentValue");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        
        var tableRepositoryOutput = _tableRepository.GetByTimeStamp(_settings.GetByTimeStamp);
        var results = tableRepositoryOutput.Result;

        var generatedModels = results.Select(oneResult => new CryptoModel()
        {
            Name = oneResult.Name,
            Price = oneResult.Price,
            CurrentDateTime = oneResult.CurrentDateTime
        }).ToList();

        var groupedList = generatedModels
            .Select(model => $"Crypto currency {model.Name} from {model.CurrentDateTime:MM/dd/yyyy HH:mm} has value of {model.Price} dollars.")
            .ToList();

        response.WriteString(string.Join(Environment.NewLine, groupedList));

        return response;
    }
}