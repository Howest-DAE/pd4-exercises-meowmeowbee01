using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField]
		private CharacterController _controller;
		[SerializeField]
		private InputActionReference _moveInput;
		[SerializeField]
		private float _moveSpeed = 3f;
		private void Start()
		{
			_moveInput.action.Enable();
		}
		void FixedUpdate()
		{
			Vector2 input = _moveInput.action.ReadValue<Vector2>();
			Vector3 moveVelocity = new Vector3(input.x, 0f, input.y) * _moveSpeed;
			Debug.Log(input);
			_controller.Move(moveVelocity * Time.fixedDeltaTime);
		}
	}
}