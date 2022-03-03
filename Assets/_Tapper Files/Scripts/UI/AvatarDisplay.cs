using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tapper.GamePlay
{
    public class AvatarDisplay : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private Image _avatarDisplay = null;
        [SerializeField] private Image _backgroundImage = null;
        public Sprite ChosenAvatar { get { return _avatarDisplay.sprite; } }
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _originalColor;
        private PlayerDisplayManager _playerDisplayManager = null;
        private int _imageIndex = 0;
        public int ImageIndex { get { return _imageIndex; } }

        #endregion Fields, Properties (end)

        #region Methods

        public void Setup(PlayerDisplayManager playerDisplayManager, Sprite avatarSprite, int imageIndex)
        {
            _playerDisplayManager = playerDisplayManager;
            _avatarDisplay.sprite = avatarSprite;
            _backgroundImage.color = _originalColor;
            _imageIndex = imageIndex;
            SetSelectedState(false);
        }

        public void OnAvatarClicked()
        {
            _playerDisplayManager.AvatarDisplay_AvatarClicked(this);
        }

        public void SetSelectedState(bool selected)
        {
            _backgroundImage.color = selected ? _selectedColor : _originalColor;
        }
        #endregion Methods (end)
    }
}