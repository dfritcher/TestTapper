using System;
using System.Collections.Generic;

namespace Tapper.Data
{
    [Serializable]
    public class PlayerData
    {
        public Guid Id;
        public string Name;
        public int ImageIndex;
        public List<GameData> Games;

        public PlayerData(GamePlay.Player player)
        {
            Id = player.Id;
            Name = player.Name;
            ImageIndex = player.ImageIndex;
            Games = new List<GameData>();
            if(player.Games != null)
            {
                foreach (var game in player.Games)
                {
                    Games.Add(new GameData(game));
                }
            }            
        }        
    }
}
