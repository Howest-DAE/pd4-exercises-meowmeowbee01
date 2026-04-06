using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PD4.ShooterGame.Presenter
{
	public class PlayerPresenter : PresenterMonoBehaviour<PlayerModel>
	{
		private GamePresenter _gamePresenter;

		[Header("Input")]
		[SerializeField]
		private InputActionReference _pickupWeaponAction;
		[SerializeField]
		private InputActionReference _dropWeaponAction;
		[SerializeField]
		private InputActionReference _fireWeaponAction;

		[Header("Pickup weapons")]
		[SerializeField]
		private float _pickupRange = 5f;

		//TODO: Replace with FollowTransform component to prevent parenting
		[SerializeField]
		private Transform _weaponAnchor;
		private Transform _pickedupWeapon = null;

		public override void OnModelPropertyChanged(string propertyName)
		{
			if (propertyName == nameof(Model.PickedUpWeapon))
			{
				ChangePickedupWeapon();
			}
		}

		private void ChangePickedupWeapon()
		{
			if (Model.PickedUpWeapon != null)
			{
				ulong weaponId = Model.PickedUpWeapon.Id;
				WeaponPresenter weapon = _gamePresenter.FindWeaponById(weaponId);
				_pickedupWeapon = weapon.transform;

				//TODO: Fake parenting using FollowTransform (can't parent NetworkObjects)
				_pickedupWeapon.SetParent(_weaponAnchor);
				_pickedupWeapon.localPosition = Vector3.zero;
				_pickedupWeapon.localRotation = Quaternion.identity;
			}
			else if (_pickedupWeapon != null)//drop weapon
			{
				//TODO: release from fake parenting
				_pickedupWeapon.SetParent(null);
				_pickedupWeapon.transform.localScale = Vector3.one;
				_pickedupWeapon = null;
			}
		}

		private void Start()
		{
			Model = new PlayerModel();

			//Add self to game presenter
			_gamePresenter = FindFirstObjectByType<GamePresenter>();
			_gamePresenter?.Model.AddPlayer(Model);

			//Register input actions
			_fireWeaponAction.action.Enable();
			_pickupWeaponAction.action.Enable();
			_dropWeaponAction.action.Enable();

			_fireWeaponAction.action.performed += FireWeaponAction_performed;
			_pickupWeaponAction.action.performed += PickupWeaponAction_performed;
			_dropWeaponAction.action.performed += DropWeaponAction_performed;

		}

		private void PickupWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon != null) return; //already holding a weapon

			WeaponPresenter nearestWeapon = _gamePresenter.FindNearestWeapon(transform.position);

			if (nearestWeapon == null) return;
			if ((transform.position - nearestWeapon.transform.position).sqrMagnitude > _pickupRange * _pickupRange) return; // too far

			Model.PickedUpWeapon = nearestWeapon.Model;

		}
		private void FireWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon == null) return; //not holding a weapon
			Model.PickedUpWeapon.Fire();

		}
		private void DropWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon == null) return; //not holding a weapon

			Model.PickedUpWeapon = null;
		}


	}
}