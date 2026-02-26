#if UNITY_SERVER
using Unity.Netcode;
#endif
using UnityEngine;

namespace Assets.Scripts
{
	public class StartServer : MonoBehaviour
	{
		private void Start()
		{
#if UNITY_SERVER
			NetworkManager.Singleton.StartServer();
			NetworkManager.Singleton.OnClientConnectedCallback += (id) => Debug.Log($"player with id:[{id}] connected");
			NetworkManager.Singleton.OnClientDisconnectCallback += (id) => Debug.Log($"player with id:[{id}] disconnected");
#endif
		}
	}
}