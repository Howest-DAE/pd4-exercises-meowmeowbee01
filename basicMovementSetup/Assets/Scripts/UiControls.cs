using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
	[RequireComponent(typeof(UIDocument))]
	public class SimpleRuntimeUI : MonoBehaviour
	{
		private Button _HostButton;
		private Button _ClientButton;

		private void OnEnable()
		{
			var uiDocument = GetComponent<UIDocument>();

			_HostButton = uiDocument.rootVisualElement.Q("Host") as Button;
			_ClientButton = uiDocument.rootVisualElement.Q("Client") as Button;

			_HostButton.RegisterCallback<ClickEvent>(HostButtonPressed);
			_ClientButton.RegisterCallback<ClickEvent>(ClientButtonPressed);
		}

		private void OnDisable()
		{
			_HostButton.UnregisterCallback<ClickEvent>(HostButtonPressed);
			_ClientButton.UnregisterCallback<ClickEvent>(ClientButtonPressed);
		}

		private static void HostButtonPressed(ClickEvent clickEvent)
		{
			NetworkManager.Singleton.StartHost();
		}

		private static void ClientButtonPressed(ClickEvent clickEvent)
		{
			NetworkManager.Singleton.StartClient();
		}
	}
}