using System;

namespace PD4.ShooterGame.Model
{
	public class WeaponSpawnEventArgs : EventArgs
	{
		public WeaponSpawnEventArgs(WeaponModel weapon, WeaponSpawnerModel spawner)
		{
			Weapon = weapon;
			Spawner = spawner;
		}
		public WeaponModel Weapon { get; }
		public WeaponSpawnerModel Spawner { get; }
	}
}