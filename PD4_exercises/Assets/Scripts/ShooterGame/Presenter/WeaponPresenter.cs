using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PD4.ShooterGame.Presenter
{
	public class WeaponPresenter : PresenterMonoBehaviour<WeaponModel>
	{
		[SerializeField] private TextMeshProUGUI _ammoText;
		[SerializeField] private Rigidbody _rigidBody;
		[SerializeField] private GameObject _muzzleFlashObject;

		private void Awake()
		{
			Model = new WeaponModel() { Ammo = 30 }; // default ammo for testing.
		}

		private void Start()
		{
			FindAnyObjectByType<GamePresenter>().Model.AddWeapon(Model);
		}

		public override void OnModelPropertyChanged(string propertyName)
		{
			if (propertyName == nameof(Model.Ammo))
			{
				UpdateAmmoText();
			}
			if (propertyName == nameof(Model.IsPickedUp))
			{
				_rigidBody.isKinematic = Model.IsPickedUp;
			}
		}

		public override void OnModelUpdated(WeaponModel previousModel)
		{
			UpdateAmmoText();
			Model.Fired += Model_Fired;
		}

		private void Model_Fired(object sender, System.EventArgs e)
		{
			// add effects like muzzle flash or sound
			StopAllCoroutines();
			StartCoroutine(ShowMuzzleFlash());
		}

		IEnumerator ShowMuzzleFlash()
		{
			_muzzleFlashObject.SetActive(true);

			yield return new WaitForSeconds(0.2f);
			_muzzleFlashObject.SetActive(false);
		}

		private void UpdateAmmoText()
		{
			_ammoText.text = $"Ammo: {Model.Ammo}";
		}
	}
}