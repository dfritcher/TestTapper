using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tapper.GamePlay
{
    public class GameSelectionManager : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private CanvasGroup _gameSelectionScreen = null;
        [SerializeField] private GameSelectionData[] _gameSelections = null;
        [SerializeField] private GameObject _gameSelectionPrefab = null;
        [SerializeField] private Transform _gameSelectionParent = null;

        private GameDirector _gameDirector = null;
        #endregion

        #region Methods
        public void Setup(GameDirector gameDirector)
        {
            _gameDirector = gameDirector;
            for(int i = 0; i < _gameSelections.Length; i++)
            {
                GetGameSelectionInstance().Setup(this, _gameSelections[i]);
            }
            SetScreenState(true);
        }

        private GameSelectionDisplay GetGameSelectionInstance()
        {
            return Instantiate(_gameSelectionPrefab, _gameSelectionParent, false).GetComponent<GameSelectionDisplay>();
        }

        internal void GameSelected(string gameName)
        {
            _gameDirector.LoadGame(gameName);
        }

        private void SetScreenState(bool isEnabled)
        {
            _gameSelectionScreen.alpha = isEnabled ? 1 : 0;
            _gameSelectionScreen.interactable = _gameSelectionScreen.blocksRaycasts = isEnabled;
        }
        #endregion
    }
}