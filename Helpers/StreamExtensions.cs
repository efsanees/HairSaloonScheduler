using System.IO;
using System.Threading.Tasks;

namespace HairSaloonScheduler.Helpers
{
	public static class StreamExtensions
	{
		public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
		{
			using (var memoryStream = new MemoryStream())
			{
				await stream.CopyToAsync(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}
