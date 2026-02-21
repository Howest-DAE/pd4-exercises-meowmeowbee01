using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
	public class SpawnSystem : NetworkBehaviour
	{
		[SerializeField]
		private SpawnLocation[] _spawnPositions;
		[SerializeField]
		private NetworkObject _playerPrefab;

		void Awake()
		{
			for (int i = 0; i < _spawnPositions.Length; i++)
			{
				_spawnPositions[i].gameObject.SetActive(false);
				int index = i; //the lambda uses the variable, not the value. therefore a local scope copy of the variable is needed
				_spawnPositions[i].Clicked += (s, e) => SpawnPlayerAvatarRpc(index, NetworkManager.Singleton.LocalClientId);
			}
		}

		public override void OnNetworkSpawn()
		{
			foreach (SpawnLocation spawnLocation in _spawnPositions)
			{
				spawnLocation.gameObject.SetActive(true);
			}
		}

		[Rpc(SendTo.Server)]
		void SpawnPlayerAvatarRpc(int spawnIndex, ulong clientId)
		{
			var transform = _spawnPositions[spawnIndex].transform;
			var spawned = Instantiate(_playerPrefab, transform.position, transform.localRotation);
			spawned.SpawnWithOwnership(clientId);
			NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject = spawned;
		}
	}
}