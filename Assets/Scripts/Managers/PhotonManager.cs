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
    }
}
