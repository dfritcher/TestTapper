using System.Collections.Generic;
using UnityEngine;

namespace Tapper.GamePlay
{
    [CreateAssetMenu(fileName = "Players", menuName ="GameInfo/Players")]
    public class PlayersContainer : ScriptableObject
    {
        public List<Player> Players;        
    }
}