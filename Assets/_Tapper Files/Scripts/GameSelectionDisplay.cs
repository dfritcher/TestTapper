using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Tapper.GamePlay
{
    public class GameSelectionDisplay : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private TextMeshProUGUI _titleDisplay = null;
        [SerializeField] private Image _screenShot = null;
        [SerializeField] private Image _playBackground = null;
        [SerializeField] private Image _playForeground = null;

        private GameSelectionManager _gameSelectionManager = null;
        private GameSelectionData _gameData = null;
        #endregion

        #region Methods
        public void Setup(GameSelectionManager gameSelectionManager, GameSelectionData gameSelectionData)
        {
            _gameSelectionManager = gameSelectionManager;
            _titleDisplay.text = gameSelectionData.Title;
            _titleDisplay.font = gameSelectionData.Font;
            _titleDisplay.color = gameSelectionData.FontColor;
            _screenShot.sprite = gameSelectionData.ScreenShot;
            _playBackground.sprite = gameSelectionData.PlayButtonBackground;
            _playForeground.sprite = gameSelectionData.PlayButtonForeground;
            _gameData = gameSelectionData;
        }

        public void OnGameClicked()
        {
            _gameSelectionManager.GameSelected(_gameData.SceneName);
        }
        #endregion
    }
}