using System;
using System.Collections.Generic;
using Tapper.Data;

namespace Tapper.GamePlay
{
    [Serializable]
    public class Player
    {
        public Guid Id;
        public string Name;
        public int ImageIndex;
        public List<Game> Games;

        public Player()
        {
            Id = Guid.NewGuid();
            Games = new List<Game>();
        }

        public Player(PlayerData playerData)
        {
            if (Guid.Empty == playerData.Id)
                Id = Guid.NewGuid();
            else
                Id = playerData.Id;            
            Name = playerData.Name;
            ImageIndex = playerData.ImageIndex;
            Games = new List<Game>();
            foreach(var game in playerData.Games)
            {
                Games.Add(new Game(game));
            }
        }
    }
}
