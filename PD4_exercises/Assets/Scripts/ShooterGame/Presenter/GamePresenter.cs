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

		public WeaponPresenter FindWeaponById(ulong id)
		{
			//TODO: find in spawned objects using networkmanager
			WeaponPresenter[] weaponsInScene = FindObjectsByType<WeaponPresenter>(FindObjectsSortMode.None);// Heavy operation, this can be optimized!
			return weaponsInScene.FirstOrDefault(w => w.Model.Id == id);
		}

		public WeaponPresenter FindNearestWeapon(Vector3 position)
		{
			WeaponPresenter[] weaponsInScene = FindObjectsByType<WeaponPresenter>(FindObjectsSortMode.None);// Heavy operation, this can be optimized!

			WeaponPresenter nearestWeapon = null;
			float nearestDistanceSq = float.MaxValue;
			foreach (var weaponPresenter in weaponsInScene)
			{
				if (weaponPresenter.Model.IsPickedUp) continue; //skip picked up weapons

				float distanceSq = (position - weaponPresenter.transform.position).sqrMagnitude;
				if (distanceSq < nearestDistanceSq)
				{
					nearestDistanceSq = distanceSq;
					nearestWeapon = weaponPresenter;
				}
			}
			return nearestWeapon;

		}
		public override void OnModelPropertyChanged(string propertyName) { }

	}
}