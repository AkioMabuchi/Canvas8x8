using System.Collections.Generic;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace Models
{
    public static class PlayerListModel
    {
        private static readonly ReactiveDictionary<int, Player> _players = new ReactiveDictionary<int, Player>();
        public static IReadOnlyReactiveDictionary<int, Player> Players => _players;

        public static void SetPlayers(IReadOnlyDictionary<int, Player> players)
        {
            foreach (KeyValuePair<int, Player> player in players)
            {
                Debug.Log("Key:" + player.Key);
                Debug.Log("Actor Number:" + player.Value.ActorNumber);
                if(_players.ContainsKey(player.Key)) continue;
                _players.Add(player.Key, player.Value);
            }
        }

        public static void UpdatePlayer(Player player)
        {
            if (_players.ContainsKey(player.ActorNumber))
            {
                
            }
            else
            {
                Debug.LogWarning("This player doesn't exist");
            }
        }

        public static void AddPlayer(Player player)
        {
            
        }

        public static void ClearPlayers()
        {
            _players.Clear();
        }
    }
}
