using System.Text;
using System.Text.Json;

namespace Integrations.Services;
public class ClientServerApi
{
    private readonly HttpClient _httpClient;

    public ClientServerApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); 
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception ex)
        {
            return $"Erro no GET para {url}: {ex.Message}";
        }
    }
    public async Task<string> PostAsync<T>(string url, T content)
    {
        try
        {
            var contentJson = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, contentJson);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception ex)
        {
            return $"Erro no POST para {url}: {ex.Message}";
        }
    }

    public async Task<string> PutAsync<T>(string url, T content)
    {
        try
        {
            var contentJson = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(url, contentJson);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception ex)
        {
            return $"Erro no PUT para {url}: {ex.Message}";
        }
    }

    public async Task<string> DeleteAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return $"Requisição DELETE para {url} foi bem-sucedida.";
        }
        catch (Exception ex)
        {
            return $"Erro no DELETE para {url}: {ex.Message}";
        }
    }
}
