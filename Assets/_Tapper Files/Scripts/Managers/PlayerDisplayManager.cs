using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tapper.GamePlay
{
    public class PlayerDisplayManager : MonoBehaviour
    {
        #region Fields, Properties
        [Header("Managers")]
        [SerializeField] private GameDirector _gameDirector = null;
        [SerializeField] private PlayerManager _playerManager = null;
        
        [Header("Scene References"), Space(8)]
        [SerializeField] private UIButton _usersButton = null;
        
        [Header("Current Player References"), Space(8)]
        [SerializeField] private GameObject _playerPrefab = null;
        [SerializeField] private Transform _playerParent = null;
        
        [Header("Screen References"), Space(8)]
        [SerializeField] private CanvasGroup _playerMainScreen = null;

        [SerializeField] private CanvasGroup _playersScreen = null;
        [SerializeField] private CanvasGroup _createPlayerScreen = null;
        
        [Header("New Player References"), Space(8)]
        [SerializeField] private TMP_InputField _newPlayerName = null;
        [SerializeField] private Transform _avatarParent = null;
        [SerializeField] private GameObject _avatarDisplayPrefab = null;
        [SerializeField] private Button _createPlayerButton = null;
        private int _selectedAvatarIndex = -1;
        #endregion

        #region Methods
        #region Unity Engine Methods
        private void Start()
        {
            _gameDirector = FindObjectOfType<GameDirector>();
            _playerManager = FindObjectOfType<PlayerManager>();
            _usersButton.OnClicked += OnUsersClicked;
            _createPlayerButton.interactable = false;
            HandleActiveUser();
            SetPlayerMainScreenState(false);
        }

        private void Update()
        {
            _createPlayerButton.interactable = !string.IsNullOrWhiteSpace(_newPlayerName.text) && _selectedAvatarIndex != -1;
        }
        #endregion Unity Engine Methods (end)

        #region Unity Canvas Called Methods
        public void OnUsersClicked(UIButton uIButton)
        {
            CreatePlayerInstances();
            CreateAvatarInstances(_playerManager.GetAllAvatars());
            SetPlayerMainScreenState(true);
            SetPlayerScreenState(true);
        }

        internal void OnPlayerSelected(PlayerDisplay playerDisplay)
        {
            _playerManager.ResetActivePlayers();
            _playerManager.SetActivePlayer(playerDisplay.Player);
            HandleActiveUser();
            OnAddPlayerBackButtonClicked();
        }

        public void OnAddPlayerBackButtonClicked()
        {
            SetPlayerMainScreenState(false);
            SetPlayerScreenState(false);
            ClearNewPlayerUI();
            ClearPlayers();
        }

        public void OnCreatePlayerBackButtonClicked()
        {
            ClearNewPlayerUI();
            SetCreatePlayerState(false);
        }

        public void OnAddNewPlayerClicked()
        {
            SetCreatePlayerState(true);
        }

        public void OnCreatePlayerClicked()
        {
            var newPlayer = new Player() { Name = _newPlayerName.text, ImageIndex = _selectedAvatarIndex };
            CreatePlayerInstance(newPlayer);
            SetCreatePlayerState(false);
            _playerManager.AddNewPlayer(newPlayer);
        }

        internal void OnPlayerDelete(PlayerDisplay playerDisplay)
        {
            _playerManager.DeletePlayer(playerDisplay.Player);
            Destroy(playerDisplay.gameObject);
        }
        #endregion Unity Canvas Called Methods (end)

        #region New Player Methods
        internal void AvatarDisplay_AvatarClicked(AvatarDisplay avatarDisplay)
        {
            _selectedAvatarIndex = avatarDisplay.ImageIndex;
            var avatars = _avatarParent.GetComponentsInChildren<AvatarDisplay>();
            foreach(var avatar in avatars)
            {
                avatar.SetSelectedState(false);
            }
            avatarDisplay.SetSelectedState(true);
        }
        #endregion New Player Methods (end)

        #region Helper Methods
        private void HandleActiveUser()
        {
            if(_playerManager.GetActivePlayers().Count > 0)
                _usersButton.SetText(_playerManager.GetActivePlayers().First().Name);
        }

        private void SetPlayerMainScreenState(bool isEnabled)
        {
            _playerMainScreen.alpha = isEnabled ? 1 : 0;
            _playerMainScreen.interactable = _playerMainScreen.blocksRaycasts = isEnabled;
        }

        private void SetPlayerScreenState(bool isEnabled)
        {
            _playersScreen.alpha = isEnabled ? 1 : 0;
            _playersScreen.interactable = _playersScreen.blocksRaycasts = isEnabled;
        }

        private void SetCreatePlayerState(bool isEnabled)
        {
            _createPlayerScreen.alpha = isEnabled ? 1 : 0;
            _createPlayerScreen.interactable = _createPlayerScreen.blocksRaycasts = isEnabled;
        }

        private void CreatePlayerInstances()
        {
            foreach (var player in _playerManager.PlayersContainer.Players)
            {
                CreatePlayerInstance(player);
            }
        }

        private void CreatePlayerInstance(Player player)
        {
            Instantiate(_playerPrefab, _playerParent, false)
                    .GetComponent<PlayerDisplay>()
                    .Setup(this, player, _playerManager.GetPlayerImage(player.ImageIndex), true);
        }

        private void CreateAvatarInstances(IEnumerable<UserImageContainer> avatars)
        {
            foreach(var avatar in avatars)
            {
                CreateAvatarInstance(avatar);
            }
        }

        private void CreateAvatarInstance(UserImageContainer imageContainer)
        {
            Instantiate(_avatarDisplayPrefab, _avatarParent, false)
                .GetComponent<AvatarDisplay>()
                .Setup(this, imageContainer.Sprite, imageContainer.ImageIndex);
        }

        private void ClearPlayers()
        {
            var players = _playerParent.GetComponentsInChildren<PlayerDisplay>();
            foreach (var player in players)
            {
                Destroy(player.gameObject);
            }
        }

        private void ClearNewPlayerUI()
        {
            _newPlayerName.text = string.Empty;
        }
        #endregion Helper Methods (end)
        #endregion
    }
}
