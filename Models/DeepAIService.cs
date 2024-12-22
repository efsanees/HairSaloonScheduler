// Services/DeepAIService.cs
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DeepAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "YOUR_DEEPAI_API_KEY"; // Buraya API anahtarınızı yazın.

    public DeepAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GenerateImageAsync(string prompt)
    {
        var requestBody = new
        {
            text = prompt
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // API'ye istek gönder
        var response = await _httpClient.PostAsync("https://api.deepai.org/api/text2img", content);

        // Yanıtı kontrol et
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
            return jsonResponse.output_url; // Resim URL'sini döndür
        }

        return "Error: Unable to generate image.";
    }
}
