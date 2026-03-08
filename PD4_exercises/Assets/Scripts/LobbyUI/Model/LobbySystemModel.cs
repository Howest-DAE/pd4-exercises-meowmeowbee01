using PD4.MVPBase.Model;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

namespace PD4.LobbySystem.Model
{
    public class LobbySystemModel : ModelBase
    {
        #region Enums
        public enum Mode
        {
            JoinLobby,
            CreateLobby,
            Ready
        }
        #endregion

        #region Fields
        private Mode _currentMode;
        private string _lobbyName;
        private Lobby _selectedLobby;
        #endregion

        #region Properties
        public Mode CurrentMode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                if (_currentMode == value) return;
                _currentMode = value;
                OnPropertyChanged();
            }
        }

        public string LobbyName
        {
            get => _lobbyName;
            set
            {
                if (_lobbyName == value)
                    return;
                _lobbyName = value;
                OnPropertyChanged();
            }
        }

        public List<Lobby> AllLobbies { get; private set; } = new List<Lobby>();

        public Lobby SelectedLobby
        {
            get => _selectedLobby;
            set
            {
                if (_selectedLobby == value)
                    return;
                _selectedLobby = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public LobbySystemModel()
        {
            AllLobbies = new List<Lobby>();
            CurrentMode = Mode.JoinLobby;
        }
        #endregion

        #region Lobby Management Methods
        public void CreateLobby()
        {
            //TODO: create lobby through LobbyManager
            CurrentMode = Mode.Ready;
        }

        public void RefreshList()
        {
            //TODO: replace lobbies with the actual data from LobbyManager
            AllLobbies.Clear();

            //TEMP FOR TESTING: Hard-coded  list of lobbies
            for (int idx = 0; idx < 10; ++idx)
            {
                AllLobbies.Add(new Lobby(name: $"Lobby {idx}"));
            }

            OnPropertyChanged(nameof(AllLobbies));
        }

        public void JoinLobby()
        {
            //TODO: Join SelectedLobby
            CurrentMode = Mode.Ready;
        }

        public void LeaveLobby()
        {
            //TODO: Leave Current Session
            CurrentMode = Mode.CreateLobby;
        }
        #endregion
    }
}