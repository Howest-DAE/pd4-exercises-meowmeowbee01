using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
	[RequireComponent(typeof(UIDocument))]
	public class UiControls : MonoBehaviour
	{
		private Button _HostButton;
		private Button _ClientButton;
		private TextField _IpField;

		private void OnEnable()
		{
			var uiDocument = GetComponent<UIDocument>();

			_HostButton = uiDocument.rootVisualElement.Q("Host") as Button;
			_ClientButton = uiDocument.rootVisualElement.Q("Client") as Button;
			_IpField = uiDocument.rootVisualElement.Q("IpField") as TextField;

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

		private void ClientButtonPressed(ClickEvent clickEvent)
		{
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_IpField.value, 7777);
			NetworkManager.Singleton.StartClient();
		}
	}
}