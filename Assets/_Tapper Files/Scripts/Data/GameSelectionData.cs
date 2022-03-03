using UnityEngine;
using TMPro;

namespace Tapper.GamePlay
{
    [CreateAssetMenu(fileName = "Game Selection", menuName = "GameInfo/Game Selection")]
    public class GameSelectionData : ScriptableObject
    {
        public Sprite ScreenShot;
        public string Title;
        public TMP_FontAsset Font;
        public Color FontColor;
        public Sprite PlayButtonBackground;
        public Sprite PlayButtonForeground;

        public string SceneName;
    }
}