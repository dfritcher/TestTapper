using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tapper.GamePlay
{
    public class PlayerManager : MonoBehaviour
    {
        #region Fields, Properties
        [SerializeField] private GameDirector _gameDirector = null;
        [SerializeField] private PlayersContainer _playersContainer = null;
        public PlayersContainer PlayersContainer { get { return _playersContainer; } }
        [SerializeField] private List<Player> _activePlayers = null;
        private List<Player> ActivePlayers { get { return _activePlayers ??= new List<Player>(); } }
        [SerializeField] private UserImageContainer[] _userImages = null;
        
        #endregion

        #region Methods
        private void Start()
        {
            _gameDirector = FindObjectOfType<GameDirector>();
            for(var i = 0; i < _userImages.Length; i++)
            {
                _userImages[i].ImageIndex = i;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadPlayerData(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                PlayersContainer.Players.Add(players[i]);
            }
        }

        internal void AddNewPlayer(Player newPlayer)
        {
            PlayersContainer.Players.Add(newPlayer);
            _gameDirector.SaveGameData();
        }

        internal List<Player> GetPlayerData()
        {
            return PlayersContainer.Players;
        }

        internal void DeletePlayer(Player player)
        {
            PlayersContainer.Players.Remove(player);
            _gameDirector.SaveGameData();
        }

        internal void SetActivePlayer(Player player)
        {
            ActivePlayers.Add(player);
        }

        public List<Player> GetActivePlayers()
        {
            return ActivePlayers;
        }

        public void ResetActivePlayers()
        {
            ActivePlayers.Clear();
        }

        public UserImageContainer[] GetAllAvatars()
        {
            return _userImages;
        }

        public Sprite GetPlayerImage(int index)
        {
            return _userImages[index].Sprite;
        }

        internal void AddNewGameForPlayer(Player player, Game game)
        {
            PlayersContainer.Players.Find(p => p.Id == player.Id).Games.Add(game);
            _gameDirector.SaveGameData();
        }

        internal void UpdateGameScore(Player player, Game game, string v)
        {
            PlayersContainer.Players.Find(p => p.Id == player.Id).Games.Find(g => g.Id == game.Id).Score = v;
            game.Score = v;
            _gameDirector.SaveGameData();
        }

        #endregion
    }
}