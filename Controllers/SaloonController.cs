using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using HairSaloonScheduler.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks.Sources;
using Microsoft.VisualStudio.Debugger.Contracts.HotReload;

namespace HairSaloonScheduler.Controllers
{
	public class SaloonController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProcessImage([Bind("image_target,hairType")] AIPhoto aiPhoto)
		{
            // 1. Dosya formatı ve boyutunu kontrol et (5 MB'den büyük olmamalı, JPEG, PNG, BMP formatları)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
			var targetExtension = Path.GetExtension(aiPhoto.image_target.FileName).ToLower();

			if (!allowedExtensions.Contains(targetExtension))
			{
				ModelState.AddModelError(string.Empty, "Invalid file format. Only JPEG, JPG, PNG, and BMP are allowed.");
				return View("Index");
			}

			if (aiPhoto.image_target.Length > 5 * 1024 * 1024)
			{
				ModelState.AddModelError(string.Empty, "File size must not exceed 5 MB.");
				return View("Index");
			}

			// 2. Çözünürlük kontrolü
			using (var targetStream = aiPhoto.image_target.OpenReadStream())
			{
				var targetImage = Image.Load(targetStream);

				if (targetImage.Width > 4096 || targetImage.Height > 4096)
				{
					ModelState.AddModelError(string.Empty, "Image resolution must be less than 4096x4096px.");
					return View("Index");
				}
			}

			// 3. API'ye dosya gönderme
			using (var content = new MultipartFormDataContent())
			{

				// image_target dosyasını ekleyelim
				var targetContent = new StreamContent(aiPhoto.image_target.OpenReadStream());
				targetContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
				content.Add(targetContent, "image_target", aiPhoto.image_target.FileName);

				// hair_type parametresini ekleyelim
				content.Add(new StringContent(aiPhoto.hairType.ToString()), "hair_type");

				// API'ye istek gönderelim
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle"),
					Headers =
					{
						{ "x-rapidapi-key", "99d27b0d02msh11c39c2f73a1290p17f98bjsnd2549c4e2a9d" },
						{ "x-rapidapi-host", "hairstyle-changer.p.rapidapi.com" }
					},
					Content = content
				};

				// Yanıtı alalım
				using (var response = await client.SendAsync(request))
				{
					var responseBody = await response.Content.ReadAsStringAsync();

					if (!response.IsSuccessStatusCode)
					{
						throw new Exception($"API Error: {response.StatusCode} - {responseBody}");
					}
					var values = JsonConvert.DeserializeObject<APIResponse>(responseBody);

					byte[] imageByteArray = Convert.FromBase64String(values.data.image);
					System.IO.File.WriteAllBytes(@"C:\Users\ASUS\source\repos\HairSaloonScheduler\wwwroot\Photos\image.png", imageByteArray);

					string imageUrl = "/Photos/image.png";
					ViewBag.ImageUrl = imageUrl;

					return View();

				}

				
			}

		}
	}
}
