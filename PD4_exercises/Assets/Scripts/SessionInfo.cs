using System;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts
{
	public class SessionInfo
	{
		private const float _HEARTBEAT_INTERVAL = 15f;
		public event EventHandler SessionEnded;
		//Properties
		public Lobby Lobby { get; private set; }
		public string LocalPlayerId { get; }
		public RelayServerData RelayServerData { get; }
		public bool IsHost { get; }
		private LobbyEventCallbacks _callbacks;
		private float _heartbeatTimer = _HEARTBEAT_INTERVAL;

		public SessionInfo(Lobby lobby, string playerId, RelayServerData relayServerData)
		{
			Lobby = lobby;
			LocalPlayerId = playerId;
			RelayServerData = relayServerData;
			IsHost = Lobby.HostId == playerId;
		}

		public async Task InitializeAsync() //Async Initialize Pattern
		{
			await UpdateLobbyInfoAsync();
			//await RegisterCallbacksAsync();
		}

		private async Task UpdateLobbyInfoAsync()
		{
			Lobby = await LobbyService.Instance.GetLobbyAsync(Lobby.Id);
		}

		public async Task UpdateSessionAsync()
		{
			_heartbeatTimer -= Time.deltaTime;
			if (IsHost && _heartbeatTimer < 0)
			{
				_heartbeatTimer += _HEARTBEAT_INTERVAL;
				await LobbyService.Instance.SendHeartbeatPingAsync(Lobby.Id);
			}
		}

		public async Task Leave()
		{
			string playerId = AuthenticationService.Instance.PlayerId;
			await LobbyService.Instance.RemovePlayerAsync(Lobby.Id, playerId);
			OnSessionEnded();
		}

		protected virtual void OnSessionEnded()
		{
			SessionEnded?.Invoke(this, EventArgs.Empty);
		}
	}
}