namespace Assignment.Interfaces;

public interface IHttpClientService
{
    Task<string> GetFromJsonAsync();
}