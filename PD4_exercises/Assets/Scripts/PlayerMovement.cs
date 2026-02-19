using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class PlayerMovement : NetworkBehaviour
	{
		[SerializeField]
		private CharacterController _controller;
		[SerializeField]
		private InputActionReference _moveInput;
		[SerializeField]
		private float _moveSpeed = 3f;
		[SerializeField]
		private float _rotationSpeed = 1000f;

		private Vector3 _moveDirection;
		private void Start()
		{
			_moveInput.action.Enable();
		}

		void FixedUpdate()
		{
			if (IsOwner) //Input is handled on the local client
			{
				Vector2 input = _moveInput.action.ReadValue<Vector2>();
				if (input.sqrMagnitude > 0f)
				{
					MovePlayerRpc(input); //Invoke RPC to send input to the Server
				}
			}

			if (IsServer)
			{
				//... apply gravity
				//auto rotate
				Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
				_rotationSpeed * Time.fixedDeltaTime);
			}
		}

		//Movement executed on the server
		[Rpc(SendTo.Server)]
		void MovePlayerRpc(Vector2 input)
		{
			//TIP: to avoid cheating, clamp the input
			Vector3 moveVelocity = new Vector3(input.x, 0f, input.y) * _moveSpeed;
			_controller.Move(moveVelocity * Time.fixedDeltaTime);
			_moveDirection = moveVelocity.normalized;
		}
	}
}