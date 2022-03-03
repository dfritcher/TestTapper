using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Tapper.GamePlay;

namespace Tapper.Data
{
    public static class FileHandler
    {
        public static void SaveData(List<GamePlay.Player> players)
        {
            var formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/playerData.bin";
            var stream = new FileStream(path, FileMode.Create);
            var playersData = new PlayerData[players.Count];
            for(int i = 0; i < players.Count; i++)
            {
                playersData[i] = new PlayerData(players[i]);
            }
            formatter.Serialize(stream, playersData);
            stream.Close();
        }

        public static GamePlay.Player[] LoadData()
        {
            string path = Application.persistentDataPath + "/playerData.bin";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                var playerData = formatter.Deserialize(stream) as PlayerData[];
                stream.Close();
                var players = new GamePlay.Player[playerData.Length];
                for(int i = 0; i < playerData.Length; i++)
                {
                    players[i] = new GamePlay.Player(playerData[i]);
                }

                return players;
            }
            else
            {
                return null;
            }
        }
    }
}
