using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Models;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace Managers
{
    public class PhotonManager : SingletonMonoBehaviourPunCallbacks<PhotonManager>
    {
        private void Start()
        {
            PhotonNetwork.GameVersion = "1.0.0";
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            RoomListModel.Update(roomList);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            
        }

        public override void OnJoinedRoom()
        {
            PlayerListModel.SetPlayers(PhotonNetwork.CurrentRoom.Players);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            
        }

        public override void OnLeftRoom()
        {
            PlayerListModel.ClearPlayers();
        }
    }
}
