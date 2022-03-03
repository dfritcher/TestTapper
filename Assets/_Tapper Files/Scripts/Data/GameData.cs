using System;
using Tapper.GamePlay;

namespace Tapper.Data
{
    [Serializable]
    public class GameData
    {
        public string Id;
        public string Name;
        public string Score;

        public GameData(Game game)
        {
            Id = game.Id;
            Name = game.Name;
            Score = game.Score;
        }
    }
}
