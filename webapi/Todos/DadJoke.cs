using System.Text.Json;
using Serilog.Core;

public class CatFactService
{
    private readonly HttpClient _httpClient;
    private readonly Logger _logger;

    public CatFactService(HttpClient httpClient, Logger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetRandomCatFact()
    {
        string apiUrl = "https://catfact.ninja/fact";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            string catFact = ParseCatFactFromJson(json);

            return catFact;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the API request
            _logger.Error($"Error retrieving dad joke: {ex.Message}");
            return string.Empty;
        }
    }

    private string ParseCatFactFromJson(string json)
    {
        JsonDocument document = JsonDocument.Parse(json);

        if (document.RootElement.TryGetProperty("fact", out JsonElement factElement))
        {
            string catFact = factElement.GetString();
            return catFact;
        }

        return string.Empty;
    }
}
