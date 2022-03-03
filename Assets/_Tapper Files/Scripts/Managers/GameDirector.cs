using System.Collections.Generic;
using UnityEngine;
using Tapper.Data;
using UnityEngine.SceneManagement;

namespace Tapper.GamePlay
{
    public class GameDirector : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private CanvasGroup _mainScreen = null;
        [SerializeField] private GameSelectionManager _gameSelectionManager = null;
        [SerializeField] private PlayerManager _playerManager = null;
        #endregion Fields, Properties (end)

        #region Methods
        private void Awake()
        {
            if (_playerManager == null)
                _playerManager = FindObjectOfType<PlayerManager>();
            LoadGameData();
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);            
        }
        
        public void SaveGameData()
        {
            FileHandler.SaveData(_playerManager.GetPlayerData());
        }

        public void LoadGameData()
        {
            var players = FileHandler.LoadData();
            if (players == null)
                return;
            _playerManager.LoadPlayerData(players);
        }

        internal void LoadGame(string gameName)
        {
            SceneManager.LoadScene(gameName);
        }

        public void OnPlayButtonClicked()
        {
            //Show Games Selection Screen
            SetScreenState(false);
            _gameSelectionManager.Setup(this);
        }
        
        private void SetScreenState(bool isEnabled)
        {
            _mainScreen.alpha = isEnabled ? 1 : 0;
            _mainScreen.interactable = _mainScreen.blocksRaycasts = isEnabled;
        }
        #endregion
    }
}