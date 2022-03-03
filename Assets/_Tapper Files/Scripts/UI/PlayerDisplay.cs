using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tapper.GamePlay
{
    public class PlayerDisplay : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private Image _playerImage = null;
        [SerializeField] private TextMeshProUGUI _playerNameDisplay = null;
        [SerializeField] private Button _deleteButton = null;
        
        private PlayerDisplayManager _playerDisplayManager = null;
        private Player _player;
        public Player Player { get { return _player; } }
        #endregion

        #region Methods
        public void Setup(PlayerDisplayManager userDisplay, Player player, Sprite playerAvatar, bool canDelete = false)
        {
            _playerDisplayManager = userDisplay;
            _player = player;
            _playerNameDisplay.text = player.Name;
            _playerImage.sprite = playerAvatar;
            _deleteButton.gameObject.SetActive(canDelete);
        }

        public void OnPlayerClicked()
        {
            _playerDisplayManager.OnPlayerSelected(this);
        }

        public void OnPlayerDeleteClicked()
        {
            _playerDisplayManager.OnPlayerDelete(this);
        }
        #endregion
    }
}