using PD4.MVPBase.Model;
using System;

namespace PD4.ShooterGame.Model
{
	public class WeaponModel : ModelBase
	{
		public ulong Id { get; set; }

		public event EventHandler Fired;
		public WeaponModel()
		{

		}

		private int _ammo;
		public int Ammo
		{
			get => _ammo;
			set
			{
				if (_ammo == value)
					return;
				_ammo = value;
				OnPropertyChanged();
			}
		}


		private bool _isPickedUp;
		public bool IsPickedUp
		{
			get => _isPickedUp;
			set
			{
				if (_isPickedUp == value)
					return;
				_isPickedUp = value;
				OnPropertyChanged();
			}
		}

		public void Pickup()
		{
			IsPickedUp = true;
		}

		public void Drop()
		{
			IsPickedUp = false;
		}

		public void Fire()
		{
			OnFired();
		}

		protected virtual void OnFired()
		{
			Fired?.Invoke(this, EventArgs.Empty);
		}
	}
}