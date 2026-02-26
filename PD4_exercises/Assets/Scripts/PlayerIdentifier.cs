using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
	[RequireComponent(typeof(MeshRenderer))]
	public class PlayerIdentifier : NetworkBehaviour
	{
		[SerializeField]
		Material hostMaterial;
		[SerializeField]
		GameObject localSphere;

		MeshRenderer meshRenderer;

		public override void OnNetworkSpawn()
		{
			NetworkManager.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<PersistentPlayer>().Player = NetworkObject;
			meshRenderer = GetComponent<MeshRenderer>();
			if (IsOwner)
			{
				Instantiate(localSphere, transform, false);
			}
			if (IsOwnedByServer)
			{
				meshRenderer.sharedMaterial = hostMaterial;
			}
		}
	}
}