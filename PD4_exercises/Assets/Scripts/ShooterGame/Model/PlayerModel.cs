using PD4.MVPBase.Model;

namespace PD4.ShooterGame.Model
{
	public class PlayerModel: ModelBase
	{

		private WeaponModel _pickedUpWeapon;

		public WeaponModel PickedUpWeapon
		{
			get => _pickedUpWeapon;
			set
			{
				if (_pickedUpWeapon == value)
					return;

				if(_pickedUpWeapon != null)
				{
					_pickedUpWeapon.Drop();
				}

				_pickedUpWeapon = value;

				if(_pickedUpWeapon != null)
				{
					_pickedUpWeapon.Pickup();
				}
				OnPropertyChanged();
			}
		}


	}
}