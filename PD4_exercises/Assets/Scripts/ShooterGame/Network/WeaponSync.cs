using PD4.ShooterGame.Model;
using Unity.Netcode;

namespace PD4.ShooterGame.Network
{
	public class WeaponSync : NetworkBehaviour
	{
		public WeaponModel Model { get; set; }
		public NetworkVariable<int> Ammo { get; set; }

		private void Awake()
		{
			Ammo = new(30);

			Ammo.OnValueChanged += (old, value) => Model.Ammo = value;
		}

		public override void OnNetworkSpawn()
		{
			Model.Id = NetworkObjectId;

			Model.Ammo = Ammo.Value;

			Model.PropertyChanged += OnAmmoChanged;
		}

		private void OnAmmoChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (!IsServer) return;
			var model = (WeaponModel)sender;
			switch (e.PropertyName)
			{
				case nameof(Model.Ammo):
					Ammo.Value = model.Ammo;
					break;
				default:
					break;
			}
		}

		[Rpc(SendTo.Server)]
		public void FireRpc()
		{
			if (Model.Ammo == 0) return;
			--Model.Ammo;
			WeaponFiredRPC();
		}

		[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server)]
		public void WeaponFiredRPC()
		{
			Model.Fire();
		}
	}
}