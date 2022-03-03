using UnityEngine;

namespace Tapper.GamePlay
{
    [CreateAssetMenu(fileName = "PlayerImages", menuName = "GameInfo/PlayerImages")]
    public class UserImageContainer : ScriptableObject
    {
        public Sprite Sprite;
        public int ImageIndex;
    }
}