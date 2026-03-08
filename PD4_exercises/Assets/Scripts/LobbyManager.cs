using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts
{
	public class LobbyManager : PD4.Singleton.MonobehaviourSingleton<LobbyManager>
	{
		private const int _MAX_PLAYERS = 3;
		SessionInfo ActiveSession { get; set; }

		private async void Awake()
		{
			await UnityServices.InitializeAsync();
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}

		public async Task<List<Lobby>> QueryLobbiesAsync()
		{
			var response = await LobbyService.Instance.QueryLobbiesAsync();
			return response.Results;
		}

		public async Task<Lobby> CreateLobbyAsync(string name)
		{
			//TODO: create relay allocation and get the join code
			Lobby createdLobby = await LobbyService.Instance.CreateLobbyAsync(name, _MAX_PLAYERS);
			return createdLobby;
		}

		public async Task JoinLobbyAsync(Lobby lobby)
		{
			Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
			//TODO: get the join code from the lobby metadata
		}
	}
}