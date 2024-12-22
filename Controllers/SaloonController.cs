using HairSaloonScheduler.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace HairSaloonScheduler.Controllers
{
    public class SaloonController : Controller
    {
        private readonly MyDbContext _context;
        private readonly DeepAIService _deepAIService;
		private readonly HttpClient _httpClient;
		public SaloonController(MyDbContext context, DeepAIService deepAIService) 
        {
            _context = context;
            _deepAIService = deepAIService;
			_httpClient = HttpClientFactory.Create();
		}

		[HttpPost]
		public async Task<IActionResult> GenerateImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return Content("No file selected");
			}

			// Fotoğrafı byte array'e çevir
			using var memoryStream = new MemoryStream();
			await file.CopyToAsync(memoryStream);
			byte[] fileBytes = memoryStream.ToArray();

			var formData = new MultipartFormDataContent
	{
		{ new ByteArrayContent(fileBytes), "file", file.FileName }
	};

			// API Key ve URL
			var apiKey = "8cf79b10-7162-4739-a699-243a5554b657";
			_httpClient.DefaultRequestHeaders.Add("Api-Key", apiKey);

			// DeepAI API'sine POST isteği gönder
			var response = await _httpClient.PostAsync("https://api.deepai.org/api/text2img", formData);

			if (response.IsSuccessStatusCode)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				try
				{
					dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
					if (jsonResponse != null && jsonResponse.output_url != null)
					{
						var imageUrl = jsonResponse.output_url;
						ViewBag.ImageUrl = imageUrl; // Sonuçları view'a gönder
					}
					else
					{
						// API'den gelen yanıt 'output_url' içermiyor, hata mesajı göster
						ViewBag.Error = "Error: No image URL returned from the AI service.";
					}
				}
				catch (Exception ex)
				{
					// JSON çözümleme hatası
					ViewBag.Error = $"Error processing API response: {ex.Message}";
				}
			}
			else
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				ViewBag.Error = $"Error generating image. API response: {responseBody}";
			}

			return View("Index");  // Sonuçları Index sayfasına yönlendiriyoruz
		}


		public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}
