using Assignment.Interfaces;
using Assignment.Models;
using Assignment.Services;
using Assignment.Storage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
        {
            services.AddAzureClients(azureBuilder =>
            {
                azureBuilder.AddTableServiceClient(context.Configuration["AzureWebJobsStorage"]);
            });
            services.Configure<MyOptions>(context.Configuration);
            services.AddHttpClient();
            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IAzureTableRepository, AzureTableRepository>();
        })
    .Build();

await host.RunAsync();