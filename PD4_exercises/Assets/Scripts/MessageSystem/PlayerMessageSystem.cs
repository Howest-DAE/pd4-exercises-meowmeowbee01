using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMessageSystem : NetworkBehaviour
{
	[SerializeField]
	SendMessagePanel _messagePanel;

	[SerializeField]
	private InputActionReference _clickAction;

	void Start()
	{
		_clickAction.action.Enable();
		_clickAction.action.performed += Action_performed;
		_messagePanel.MessageSent += _messagePanel_MessageSent;
	}

	private void Action_performed(InputAction.CallbackContext obj)
	{
		Vector3 mousePos = Mouse.current.position.ReadValue();
		mousePos.z = Camera.main.farClipPlane;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hitInfo, Camera.main.farClipPlane))
		{
			if (hitInfo.collider.CompareTag("Player"))
			{
				PlayerMessageBubble targetMessageBubble = hitInfo.collider.GetComponent<PlayerMessageBubble>();

				//Testing: Directly show message on the clicked object
				//targetMessageBubble.ShowMessage("Ouch!");
				//ex. 1: show the message on the clicked client!
				//targetMessageBubble.ShowMessageRpc("Ouch!");
				//Show the MessagePanel with the correct playerIds
				_messagePanel.Show(targetMessageBubble.OwnerClientId, NetworkManager.Singleton.LocalClientId);
			}
		}
	}

	private void _messagePanel_MessageSent(object sender, SendMessagePanel.MessageEventArgs e)
	{
		Debug.Log($"I should send a secret message \"{e.Message}\" to player {e.TargetPlayerId}");
		//send the message to the player, make sure it only arrives on the system of the target player
		ReceivedSecretMessageRpc(e.Message, e.SourcePlayerId, NetworkManager.Singleton.RpcTarget.Single(e.TargetPlayerId, RpcTargetUse.Temp));
	}

	[Rpc(SendTo.SpecifiedInParams)]
	private void ReceivedSecretMessageRpc(string message, ulong fromPlayerId, RpcParams rpcParams)
	{
		Debug.Log($"I received a secret message \"{message}\" from player {fromPlayerId}");
		PlayerMessageBubble targetMessageBubble = NetworkManager.Singleton.ConnectedClients[fromPlayerId].PlayerObject.GetComponent<PlayerMessageBubble>();
		targetMessageBubble.ShowMessage(message);
	}
}
