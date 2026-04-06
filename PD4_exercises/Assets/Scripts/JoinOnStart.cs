using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Assets.Scripts
{
	public class JoinOnStart : MonoBehaviour
	{
		[SerializeField]
		UnityTransport transport;
		void Start()
		{
			if (LobbyManager.Instance.ActiveSession == null) return;
			transport.SetRelayServerData(LobbyManager.Instance.ActiveSession.RelayServerData);
			if (LobbyManager.Instance.ActiveSession.IsHost)
			{
				NetworkManager.Singleton.StartHost();
				Debug.Log("started as host");
			}
			else NetworkManager.Singleton.StartClient();
		}
	}
}