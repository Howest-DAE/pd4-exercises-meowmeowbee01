using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.AsyncExercises
{
	public class DisplayLogo : MonoBehaviour
	{
		readonly List<string> _urls = new()
		{
			"https://www.howest.be/fs/styles/fixed_medium_nocrop/public/images/howest-hogeschool-logo-poweredby.png",
			"https://www.howest.be/fs/styles/fixed_medium_nocrop/public/images/howest-university-of-applied-sciences-logo.png",
			"https://www.howest.be/fs/styles/fixed_medium_nocrop/public/images/howest-hogeschool-logo.png"
		};

		int _urlIndex = 0;

		[SerializeField]
		UIDocument _uiDocument;

		private async void Start()
		{
			VisualElement logoElement = _uiDocument.rootVisualElement.Q("logoImage");
			var button = _uiDocument.rootVisualElement.Q("nextButton") as Button;

			button.RegisterCallback<ClickEvent>(async e => await NextTexture(logoElement));

			await LoadTextureAsync(logoElement, _urls[_urlIndex]);
		}

		private async Task NextTexture(VisualElement logo)
		{
			_urlIndex = (_urlIndex + 1) % _urls.Count;
			await LoadTextureAsync(logo, _urls[_urlIndex]);
		}

		private async Task LoadTextureAsync(VisualElement element, string url)
		{
			TextureLoader loader = new();
			Texture2D texture = await loader.LoadTextureAsync(url);
			element.style.backgroundImage = texture;
		}
	}
}