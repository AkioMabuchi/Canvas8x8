using System;
using Photon.Realtime;
using UnityEngine;

namespace GameStates
{
    public abstract class GameState
    {
        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public virtual void OnConnectedToMaster()
        {
            
        }

        public virtual void OnJoinedLobby()
        {
            
        }

        public virtual void OnDisconnected(DisconnectCause cause)
        {
            
        }
    }
}
