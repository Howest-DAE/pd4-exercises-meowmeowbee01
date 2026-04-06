using UnityEngine;
using UnityEngine.UIElements;

namespace PD4.ShooterGame.View
{
	public class CounterUIView : MonoBehaviour
	{
		[SerializeField]
		private UIDocument _counterDoc;

		private TextElement _counterText;
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			_counterText = _counterDoc.rootVisualElement.Q<TextElement>("countTxt");
		}

		public void SetValue(int text)
		{
			_counterText.text = text.ToString();
		}

	}
}
