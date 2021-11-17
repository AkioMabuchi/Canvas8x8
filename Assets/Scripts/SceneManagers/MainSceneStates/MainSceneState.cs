using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace SceneManagers.MainSceneStates
{ 
    public abstract class MainSceneState
    {
        public virtual void OnChangedRoomState()
        {
            
        }
        public virtual void OnClickButtonExit()
        {
            
        }

        public virtual void OnClickButtonReady()
        {
            
        }

        public virtual void OnPlayerLeftRoom()
        {
            
        }

        protected bool CheckRoomState(RoomState state)
        {
            if (!PhotonNetwork.InRoom) return false;
            var hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            if (!hashtable.ContainsKey("State")) return false;
            if (hashtable["State"] is RoomState roomState)
            {
                return roomState == state;
            }

            return false;
        }
    }
}
