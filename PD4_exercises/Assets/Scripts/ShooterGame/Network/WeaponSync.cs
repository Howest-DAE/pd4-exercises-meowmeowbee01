using PD4.ShooterGame.Model;
using Unity.Netcode;

namespace PD4.ShooterGame.Network
{
	public class WeaponSync : NetworkBehaviour
	{
		public WeaponModel Model { get; set; }
		public NetworkVariable<ulong> Id { get; set; }

		//Fired?
	}
}