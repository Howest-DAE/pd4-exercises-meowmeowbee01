using System;

namespace PD4.ShooterGame.Model
{
	public class WeaponEventArgs : EventArgs
	{
		public WeaponEventArgs(WeaponModel weapon)
		{
			Weapon = weapon;
		}

		public WeaponModel Weapon { get; }
	}
}