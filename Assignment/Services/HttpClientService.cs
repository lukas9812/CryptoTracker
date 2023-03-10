using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assignment.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly MyOptions _settings;

    public HttpClientService(IHttpClientFactory httpClientFactory, IOptions<MyOptions> settings)
    {
        _settings = settings.Value;
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<string> GetFromJsonAsync()
    {
        _httpClient.BaseAddress = new Uri($"https://rest-sandbox.coinapi.io/v1/assets/{_settings.CryptoCurrencyName}/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new
                System.Net.Http.Headers.AuthenticationHeaderValue(_settings.ApiKeyToCoinApi);

        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
            
        return response.Content.ReadAsStringAsync().Result;
    }
}