using System;
using Tapper.Data;

namespace Tapper.GamePlay
{
    [Serializable]
    public class Game
    {
        public string Id;
        public string Name;
        public string Score;

        public Game()
        {

        }

        public Game(GameData gameData)
        {
            if (gameData == null)
                return;
            else
            {
                Id = gameData.Id;
                Name = gameData.Name;
                Score = gameData.Score;
            }                
        }        
    }
}
