using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
	public class PlayerCounter : NetworkBehaviour
	{
		private NetworkVariable<int> _counter = new(0, writePerm: NetworkVariableWritePermission.Owner);
		[SerializeField]
		TextMeshProUGUI _countText;
		[SerializeField]
		InputActionReference _inputAction;

		public override void OnNetworkSpawn()
		{
			if (IsOwner)
			{
				_inputAction.action.performed += Action_performed;
			}
			_counter.OnValueChanged += UpdateUItext;
			UpdateUItext();
		}

		private void Action_performed(InputAction.CallbackContext obj)
		{
			_counter.Value++;
			UpdateUItext();
		}
		private void UpdateUItext()
		{
			_countText.text = _counter.Value.ToString();
		}
		private void UpdateUItext(int previousValue, int newValue)
		{
			_countText.text = newValue.ToString();
		}
	}
}