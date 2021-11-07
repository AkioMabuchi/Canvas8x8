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
        private readonly Subject<Unit> _connectedToMaster = new Subject<Unit>();
        public IObservable<Unit> ConnectedToMaster => _connectedToMaster;

        private readonly Subject<Unit> _joinedRoom = new Subject<Unit>();
        public IObservable<Unit> JoinedRoom => _joinedRoom;
        
        private readonly Subject<int> _joinRoomFailed = new Subject<int>();
        public IObservable<int> JoinRoomFailed => _joinRoomFailed;
        
        private readonly Subject<DisconnectCause> _disconnected = new Subject<DisconnectCause>();
        public IObservable<DisconnectCause> Disconnected => _disconnected;

        private void Start()
        {
            PhotonNetwork.GameVersion = "1.0.0";
        }

        public override void OnConnectedToMaster()
        {
            _connectedToMaster.OnNext(Unit.Default);
        }
        
        public override void OnJoinedRoom()
        {
            _joinedRoom.OnNext(Unit.Default);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _joinRoomFailed.OnNext(returnCode);
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _disconnected.OnNext(cause);
        }
    }
}
