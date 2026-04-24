using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using System.Linq;
using UnityEngine;

namespace PD4.ShooterGame.Presenter
{
	public class GamePresenter : PresenterMonoBehaviour<GameModel>
	{
		private void Awake()
		{
			Model = new GameModel();
		}

		static public WeaponPresenter FindWeaponById(ulong id)
		{
			//TODO: find in spawned objects using networkmanager
			WeaponPresenter[] weaponsInScene = FindObjectsByType<WeaponPresenter>(FindObjectsSortMode.None);// Heavy operation, this can be optimized!
			return weaponsInScene.FirstOrDefault(w => w.Model.Id == id);
		}

		static public WeaponPresenter? FindNearestWeapon(Vector3 position)
		{
			WeaponPresenter[] weaponsInScene = FindObjectsByType<WeaponPresenter>(FindObjectsSortMode.None);// Heavy operation, this can be optimized!

			// doesn't work because it only returns the distance, not the weapon
			//weaponsInScene
			//	.Where(wp => !wp.Model.IsPickedUp)
			//	.Min(wp => (position - wp.transform.position).sqrMagnitude);

			// O(n log(n)) when O(n) is possible
			//weaponsInScene
			//	.Where(wp => !wp.Model.IsPickedUp)
			//	.OrderBy(wp => (position - wp.transform.position).sqrMagnitude)
			//	.FirstOrDefault();

			//less readable, but O(n)
			var pickableWeapons = weaponsInScene
				.Where(wp => !wp.Model.IsPickedUp);
			return !pickableWeapons.Any() ? null : pickableWeapons
				.Aggregate((wp1, wp2) => wp1.DistanceSq(position) < wp2.DistanceSq(position) ? wp1 : wp2);

			//O(n), more code
			//WeaponPresenter nearestWeapon = null;
			//float nearestDistanceSq = float.MaxValue;
			//foreach (var weaponPresenter in weaponsInScene)
			//{
			//	if (weaponPresenter.Model.IsPickedUp) continue; //skip picked up weapons

			//	float distanceSq = (position - weaponPresenter.transform.position).sqrMagnitude;
			//	if (distanceSq < nearestDistanceSq)
			//	{
			//		nearestDistanceSq = distanceSq;
			//		nearestWeapon = weaponPresenter;
			//	}
			//}
			//return nearestWeapon;
		}
		public override void OnModelPropertyChanged(string propertyName) { }
	}
}