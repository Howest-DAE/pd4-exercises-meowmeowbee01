using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.AsyncExercises
{
	public class TextureLoader
	{
		public async Task<Texture2D> LoadTextureAsync(string url)
		{
			var request = UnityWebRequestTexture.GetTexture(url);
			await request.SendWebRequest();
			return DownloadHandlerTexture.GetContent(request);
		}
	}
}