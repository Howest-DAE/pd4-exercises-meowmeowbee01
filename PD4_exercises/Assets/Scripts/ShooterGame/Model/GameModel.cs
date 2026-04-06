using PD4.MVPBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PD4.ShooterGame.Model
{

	public class GameModel : ModelBase
	{
		public List<WeaponModel> Weapons { get; } = new List<WeaponModel>();
		public List<PlayerModel> Players { get; } = new List<PlayerModel>();
		public List<WeaponSpawnerModel> Spawners { get; } = new List<WeaponSpawnerModel>();

		public void AddWeaponSpawner(WeaponSpawnerModel spawner)
		{
			Spawners.Add(spawner);
		}
		public void AddWeapon(WeaponModel weapon)
		{
			Weapons.Add(weapon);
		}
		public void AddPlayer(PlayerModel player)
		{
			Players.Add(player);
		}

		public WeaponModel GetWeapon(ulong id)
		{
			return null;
		}

		public PlayerModel GetPlayer(ulong id)
		{
			return null;
		}

		public WeaponSpawnerModel GetSpawner(ulong id)
		{
			return null;
		}
	}
}