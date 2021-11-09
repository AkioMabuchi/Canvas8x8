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
    public enum PlayerStatus
    {
        NotReady,
        Ready,
        Examiner,
        Answerer,
    }

    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.GameVersion = "1.0.0";
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.NickName = UserNameModel.UserName.Value;

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
            PlayerListModel.UpdatePlayer(targetPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            PlayerListModel.AddPlayer(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PlayerListModel.RemovePlayer(otherPlayer);
        }

        public override void OnLeftRoom()
        {
            PlayerListModel.ClearPlayers();
        }
    }
}