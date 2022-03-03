using UnityEngine;
using UnityEngine.EventSystems;

namespace Tapper.GamePlay.BalloonPop
{
    public class TapHandler : UIButton
    {
        [SerializeField] private BalloonGameManager _gameManager = null;

        private int _tapCount = 0;

        public override void Reset()
        {
            _tapCount = 0;
            base.Reset();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (_gameManager.IsRoundEnded)
                return;
            _audioSource.PlayOneShot(_clickedAudioClip);
            _transform.transform.localScale = _originalSize;
            _tapCount++;
            _gameManager.TapHandler(_tapCount);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _transform.transform.localScale = _pushSize;
        }
    }
}