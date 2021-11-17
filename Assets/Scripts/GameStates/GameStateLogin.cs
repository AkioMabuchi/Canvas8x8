using Dialogs;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GameStates
{
    public class GameStateLogin : GameState
    {
        public override void OnEnter()
        {
            Dialogs.Dialogs.Instance.Show();
            DialogConnectingServer.Instance.Show();
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnExit()
        {
            
        }

        public override void OnConnectedToMaster()
        {
            DialogConnectingServer.Instance.Hide();
            PhotonNetwork.JoinLobby();
            DialogJoiningLobby.Instance.Show();
        }

        public override void OnJoinedLobby()
        {
            DialogJoiningLobby.Instance.Hide();
            Dialogs.Dialogs.Instance.Hide();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            
        }
    }
}
