using Assets.Scripts;
using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using PD4.ShooterGame.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PD4.ShooterGame.Presenter
{
	[RequireComponent(typeof(PlayerSync))]
	public class PlayerPresenter : PresenterMonoBehaviour<PlayerModel>
	{
		private GamePresenter _gamePresenter;
		private PlayerSync _playerSync;

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

		// Replace with FollowTransform component to prevent parenting
		[SerializeField]
		private FollowTransform _weaponAnchor;
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
				WeaponPresenter weapon = GamePresenter.FindWeaponById(weaponId);
				_pickedupWeapon = weapon.transform;

				// Fake parenting using FollowTransform (can't parent NetworkObjects)
				_weaponAnchor.AddChild(_pickedupWeapon);
				_pickedupWeapon.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			}
			else if (_pickedupWeapon != null)//drop weapon
			{
				// release from fake parenting
				_weaponAnchor.RemoveChild(_pickedupWeapon);
				_pickedupWeapon.transform.localScale = Vector3.one;
				_pickedupWeapon = null;
			}
		}

		private void Awake()
		{
			Model = new PlayerModel();

			_playerSync = GetComponent<PlayerSync>();
			_playerSync.Model = Model;
		}

		private void Start()
		{
			//Add self to game presenter
			_gamePresenter = FindFirstObjectByType<GamePresenter>();
			_gamePresenter?.Model.AddPlayer(Model);

			if (_playerSync.IsOwner)
			{
				//Register input actions
				_fireWeaponAction.action.Enable();
				_pickupWeaponAction.action.Enable();
				_dropWeaponAction.action.Enable();

				_fireWeaponAction.action.performed += FireWeaponAction_performed;
				_pickupWeaponAction.action.performed += PickupWeaponAction_performed;
				_dropWeaponAction.action.performed += DropWeaponAction_performed;
			}

		}

		private void PickupWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon != null) return; //already holding a weapon

			var nearestWeapon = GamePresenter.FindNearestWeapon(transform.position);

			if (nearestWeapon == null) return;
			if (nearestWeapon.DistanceSq(transform.position) > _pickupRange * _pickupRange) return; // too far

			_playerSync.SetWeaponRpc(nearestWeapon.Model.Id);
		}

		private void FireWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon == null) return; //not holding a weapon
			_playerSync.FireWeaponRpc();
		}

		private void DropWeaponAction_performed(InputAction.CallbackContext context)
		{
			if (Model.PickedUpWeapon == null) return; //not holding a weapon
			_playerSync.DropWeaponRpc();
		}
	}
}