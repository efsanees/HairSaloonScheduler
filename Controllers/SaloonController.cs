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

namespace HairSaloonScheduler.Controllers
{
	public class SaloonController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View(new ImageViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> ProcessImage(IFormFile photo, IFormFile image_target, int hairType = 101)
		{
			// 1. Dosya formatı ve boyutunu kontrol et (5 MB'den büyük olmamalı, JPEG, PNG, BMP formatları)
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
			var photoExtension = Path.GetExtension(photo.FileName).ToLower();
			var targetExtension = Path.GetExtension(image_target.FileName).ToLower();

			if (!allowedExtensions.Contains(photoExtension) || !allowedExtensions.Contains(targetExtension))
			{
				ModelState.AddModelError(string.Empty, "Invalid file format. Only JPEG, JPG, PNG, and BMP are allowed.");
				return View("Index");
			}

			if (photo.Length > 5 * 1024 * 1024 || image_target.Length > 5 * 1024 * 1024)
			{
				ModelState.AddModelError(string.Empty, "File size must not exceed 5 MB.");
				return View("Index");
			}

			// 2. Çözünürlük kontrolü
			using (var photoStream = photo.OpenReadStream())
			using (var targetStream = image_target.OpenReadStream())
			{
				var photoImage = Image.Load(photoStream);
				var targetImage = Image.Load(targetStream);

				if (photoImage.Width > 4096 || photoImage.Height > 4096 || targetImage.Width > 4096 || targetImage.Height > 4096)
				{
					ModelState.AddModelError(string.Empty, "Image resolution must be less than 4096x4096px.");
					return View("Index");
				}
			}

			// 3. API'ye dosya gönderme
			using (var content = new MultipartFormDataContent())
			{
				// photo dosyasını ekleyelim
				var photoContent = new StreamContent(photo.OpenReadStream());
				photoContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
				content.Add(photoContent, "image", photo.FileName);

				// image_target dosyasını ekleyelim
				var targetContent = new StreamContent(image_target.OpenReadStream());
				targetContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
				content.Add(targetContent, "image_target", image_target.FileName);

				// hair_type parametresini ekleyelim
				content.Add(new StringContent(hairType.ToString()), "hair_type");

				// API'ye istek gönderelim
				var client = new HttpClient();
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://hairstyle-changer.p.rapidapi.com/huoshan/facebody/hairstyle"),
					Headers =
					{
						{ "x-rapidapi-key", "85e7eeb9b2msh2ebbe5c68240f1fp1e0f42jsn4c9859daa16f" },
						{ "x-rapidapi-host", "hairstyle-changer.p.rapidapi.com" }
					},
					Content = content
				};

				// Yanıtı alalım
				using (var response = await client.SendAsync(request))
				{
					var responseBody = await response.Content.ReadAsStringAsync();

					// Yanıtı konsola yazdırma (debugging için)
					Console.WriteLine("API Response: " + responseBody);

					if (!response.IsSuccessStatusCode)
					{
						throw new Exception($"API Error: {response.StatusCode} - {responseBody}");
					}

					// JSON yanıtını JObject olarak çözümleyelim
					var responseData = JObject.Parse(responseBody);

					// Yanıtın tamamını yazdıralım, böylece yapıyı görebiliriz
					Console.WriteLine("Parsed JSON: " + responseData.ToString());

					// Hata kodunu kontrol edelim
					var errorCode = (int)responseData["error_code"];
					if (errorCode != 0)
					{
						var errorMessage = responseData["error_detail"]?["message"]?.ToString();
						ModelState.AddModelError(string.Empty, $"API Error: {errorMessage}");
						return View("Index");
					}

					// 'data' alanının var olup olmadığını kontrol et
					var dataToken = responseData["data"];
					if (dataToken == null)
					{
						ModelState.AddModelError(string.Empty, "Data not found in the response.");
						return View("Index");
					}

					// 'image' alanının var olup olmadığını kontrol et
					var imageToken = dataToken["image"];
					if (imageToken == null)
					{
						ModelState.AddModelError(string.Empty, "Image data not found.");
						return View("Index");
					}

					// Orijinal resmin Base64 dönüşümü
					string base64Photo;
					using (var ms = new MemoryStream())
					{
						base64Photo = Convert.ToBase64String(await photo.OpenReadStream().ToByteArrayAsync());
					}

					// ViewModel'e ekleyelim
					var viewModel = new ImageViewModel
					{
						OriginalImage = base64Photo,
						ProcessedImage = imageToken.ToString()
					};

					return View("Index", viewModel);
				}

			}
		}
	}
}
