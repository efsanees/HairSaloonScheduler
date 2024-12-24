namespace HairSaloonScheduler.Models
{
	public class ImageViewModel
	{
		public string OriginalImage { get; set; } // Yüklenen fotoğraf (Base64 formatında)
		public string ProcessedImage { get; set; } // API'den gelen işlenmiş fotoğraf (Base64 formatında)
	}
}
