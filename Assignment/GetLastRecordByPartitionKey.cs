using System.Collections.Generic;
using System.Net;
using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assignment;

public class GetLastRecordByPartitionKey
{
    private readonly IAzureTableRepository _tableRepository;
    private readonly MyOptions _settings;

    public GetLastRecordByPartitionKey(IAzureTableRepository tableRepository, IOptions<MyOptions> settings)
    {
        _tableRepository = tableRepository;
        _settings = settings.Value;
    }

    [Function("GetLastRecordByPartitionKey")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("GetLastRecordByPartitionKey");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        var lastInsertedRecordDo = _tableRepository.GetLastRecordByPartitionKey(_settings.PartitionKey).Result;
        
        var domainObject = new CryptoModel
        {
            Name = lastInsertedRecordDo.Name,
            Price = lastInsertedRecordDo.Price,
            CurrentDateTime = lastInsertedRecordDo.CurrentDateTime
        };
        logger.LogInformation("Last inserted data were acquired successfully");

        var messageToUser = $"Last known {domainObject.Name} value is {domainObject.Price} dollars. ({domainObject.CurrentDateTime:MM/dd/yyyy HH:mm})";
        
        response.WriteString(messageToUser);

        return response;
        
    }
}