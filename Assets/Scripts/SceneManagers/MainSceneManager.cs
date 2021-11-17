using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Models;
using Photon.Pun;
using Photon.Realtime;
using SceneManagers.MainSceneStates;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum RoomState
{
    None,
    Idle,
    GameInitialize,
    GameStart,
    RoundStart,
    MainTime,
    Answered,
    TimeUp,
    NextRound,
    GameEnd,
    GameFinish,
    Error
}

namespace SceneManagers
{
    public class MainSceneManager : MonoBehaviourPunCallbacks
    {
        private readonly Dictionary<RoomState, MainSceneState> _mainSceneStates =
            new Dictionary<RoomState, MainSceneState>
            {
                {RoomState.Idle, new MainSceneStateIdle()},
                {RoomState.GameInitialize, new MainSceneStateGameInitialize()}
            };

        private MainSceneState _mainSceneState = new MainSceneStateIdle();

        private void Start()
        {
            SceneController.Instance.SetCurrentSceneName("MainScene");
            
            CanvasForceHalt.Instance.Hide();
            CanvasCalls.Instance.HideImageCall();
            CanvasCalls.Instance.HideTextCall();
            CanvasPallet.Instance.Hide();
            CanvasAnswer.Instance.Hide();
            CanvasTimer.Instance.SetCountTextByInt(0);
            AnswerInputModel.Clear();
            CanvasTheme.Instance.InitializeText();

            CanvasMain.Instance.OnClickButtonExit.Subscribe(_ =>
            {
                _mainSceneState.OnClickButtonExit();
            }).AddTo(gameObject);

            CanvasMain.Instance.OnClickButtonReady.Subscribe(_ =>
            {
                _mainSceneState.OnClickButtonReady();
            }).AddTo(gameObject);
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _mainSceneState.OnPlayerLeftRoom();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneController.Instance.ChangeScene("LobbyScene");
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey("State"))
            {
                if (propertiesThatChanged["State"] is RoomState roomState)
                {
                    _mainSceneState = _mainSceneStates[roomState];
                    _mainSceneState.OnChangedRoomState();
                }
            }

            if (true)
            {
                
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {

        }
    }
}

