using PD4.ShooterGame.Model;
using PD4.ShooterGame.Presenter;
using Unity.Netcode;

namespace PD4.ShooterGame.Network
{
	public class PlayerSync : NetworkBehaviour
	{
		public PlayerModel Model { get; set; }
		public NetworkVariable<ulong> WeaponId { get; set; }

		private void Awake()
		{
			WeaponId = new();

			WeaponId.OnValueChanged += (old, value) => Model.PickedUpWeapon = GamePresenter.FindWeaponById(value)?.Model;
		}

		public override void OnNetworkSpawn()
		{
			Model.PickedUpWeapon = GamePresenter.FindWeaponById(WeaponId.Value)?.Model;

			Model.PropertyChanged += OnWeaponChanged;
		}

		private void OnWeaponChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (!IsServer) return;
			var model = (PlayerModel)sender;
			switch (e.PropertyName)
			{
				case nameof(Model.PickedUpWeapon):
					WeaponId.Value = model.PickedUpWeapon?.Id ?? 0;
					break;
				default:
					break;
			}
		}

		[Rpc(SendTo.Everyone)]
		public void SetWeaponRpc(ulong id)
		{
			Model.PickedUpWeapon = GamePresenter.FindWeaponById(id)?.Model;
		}

		[Rpc(SendTo.Server)]
		public void FireWeaponRpc()
		{
			var weaponPresenter = GamePresenter.FindWeaponById(Model.PickedUpWeapon.Id);
			weaponPresenter.Sync.FireRpc();
		}

		[Rpc(SendTo.Everyone)]
		public void DropWeaponRpc()
		{
			Model.PickedUpWeapon = null;
		}
	}
}