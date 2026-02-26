using Unity.Netcode;

namespace Assets.Scripts
{
	public class PersistentPlayer : NetworkBehaviour
	{
		public NetworkObject Player { get; set; }
	}
}