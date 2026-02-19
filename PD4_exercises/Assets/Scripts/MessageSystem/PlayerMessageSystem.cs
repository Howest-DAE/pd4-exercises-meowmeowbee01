using System.Linq;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMessageSystem : MonoBehaviour   
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
				targetMessageBubble.ShowMessage("Ouch!");
				//TODO - ex. 1: show the message on the clicked client!

				//TODO: Show the MessagePanel with the correct playerIds
				//_messagePanel.Show(0, 0);
			}
		}
	}

	private void _messagePanel_MessageSent(object sender, SendMessagePanel.MessageEventArgs e)
	{
		//TODO: send the message to the player, make sure it only arrives on the system of the target player
		Debug.Log($"I should send a secret message \"{e.Message}\" to player {e.TargetPlayerId}");
	}

}
