using System;
using System.Collections.Generic;
using Canvases;
using ExitGames.Client.Photon;
using Managers;
using Models;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace SceneManagers
{
    public class MainSceneManager : MonoBehaviourPunCallbacks
    {
        private readonly Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();

        private bool _isPlaying;
        private bool _isReady;
        private void Start()
        {
            SceneController.Instance.SetCurrentSceneName("MainScene");
            CanvasMain.Instance.SetButtonExitInteractable(false);
            CanvasMain.Instance.SetButtonReadyInteractable(false);

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.SetPlayerCustomProperties(new Hashtable
                {
                    {"Status", PlayerStatus.NotReady},
                    {"Answer", ""}
                });
                _disposables.Add(
                    "PlayerListAdd",
                    PlayerListModel.Players.ObserveAdd().Subscribe(_ =>
                {
                    CanvasPlayerList.Instance.UpdateMessages(PlayerListModel.GetPlayerList());
                    MainGameStart();
                }));
                
                _disposables.Add(
                    "PlayerListRemove",
                    PlayerListModel.Players.ObserveRemove().Subscribe(_ =>
                {
                    CanvasPlayerList.Instance.UpdateMessages(PlayerListModel.GetPlayerList());
                    MainGameStart();
                }));
                
                _disposables.Add(
                    "PlayerListReplace",
                    PlayerListModel.Players.ObserveReplace().Subscribe(_ =>
                {
                    CanvasPlayerList.Instance.UpdateMessages(PlayerListModel.GetPlayerList());
                    MainGameStart();
                }));
                
                _disposables.Add(
                    "PlayerListReset",
                    PlayerListModel.Players.ObserveReset().Subscribe(_ =>
                {
                    CanvasPlayerList.Instance.UpdateMessages(PlayerListModel.GetPlayerList());
                }));

                _isReady = false;
                EnableButtonExit();
                EnableButtonReady();
            }
            else
            {
                SceneController.Instance.ChangeScene("LobbyScene");
            }
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables.Values) disposable.Dispose();
        }

        void EnableButtonExit()
        {
            _disposables.Add(
                "OnClickButtonExit",
                CanvasMain.Instance.OnClickButtonExit.Subscribe(_ =>
                {
                    PhotonNetwork.LeaveRoom();
                }));
            CanvasMain.Instance.SetButtonExitInteractable(true);
        }

        void DisableButtonExit()
        {
            Dispose("OnClickButtonExit");
            CanvasMain.Instance.SetButtonExitInteractable(false);
        }

        void EnableButtonReady()
        {
            _disposables.Add(
                "OnClickButtonReady",
                CanvasMain.Instance.OnClickButtonReady.Subscribe(_ =>
                {
                    if (_isReady)
                    {
                        SetPlayerProperty("Status", PlayerStatus.NotReady);
                        _isReady = false;
                        EnableButtonExit();
                    }
                    else
                    {
                        SetPlayerProperty("Status", PlayerStatus.Ready);
                        _isReady = true;
                        DisableButtonExit();
                    }

                    CanvasMain.Instance.ChangeButtonReadyImage(_isReady);
                }));
            CanvasMain.Instance.SetButtonReadyInteractable(true);
        }

        void DisableButtonReady()
        {
            Dispose("OnClickButtonReady");
            CanvasMain.Instance.SetButtonReadyInteractable(false);
        }

        /*
        private void ChangeMode(Mode mode)
        {

            switch (mode)
            {
                case Mode.Initial:
                {
                    break;
                }
                case Mode.Waiting:
                {
                    break;
                }
                case Mode.Examiner:
                {
                    break;
                }
                case Mode.Answerer:
                {
                    _disposables.Add(AnswerInputModel.InputText.Subscribe(text =>
                    {
                        CanvasAnswer.Instance.SetText(text);
                    }));
                    
                    _disposables.Add(InputAnswerManager.Instance.OnInputKey.Subscribe(inputChar =>
                    {
                        switch (inputChar)
                        {
                            case '\r':
                            {
                                if (true)
                                {
                                    AnswerInputModel.Clear();
                                }
                                break;
                            }
                            case '\b':
                            {
                                AnswerInputModel.Delete();
                                break;
                            }
                            default:
                            {
                                if (inputChar == '\0')
                                {
                                    break;
                                }

                                AnswerInputModel.Input(inputChar);
                                break;
                            }
                        }
                    }));
                    break;
                }
            }
        }
        */
        
        void SetRoomProperty(string key, object value)
        {
            Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            hashtable[key] = value;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }

        void SetPlayerProperty(string key, object value)
        {
            Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
            hashtable[key] = value;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
        
        private void Dispose(string key)
        {
            if (_disposables.ContainsKey(key))
            {
                _disposables[key].Dispose();
                _disposables.Remove(key);
            }
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            
        }
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneController.Instance.ChangeScene("LobbyScene");
        }

        public void MainGameStart()
        {
            if (!_isPlaying && PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
            {
                bool isEverybodyReady = true;
                IReadOnlyList<Player> players = PlayerListModel.GetPlayerList();
                foreach (Player player in players)
                {
                    if (player.CustomProperties.ContainsKey("Status"))
                    {
                        if ((PlayerStatus) player.CustomProperties["Status"] != PlayerStatus.Ready)
                        {
                            isEverybodyReady = false;
                        }
                    }
                    else
                    {
                        isEverybodyReady = false;
                    }
                }

                if (isEverybodyReady)
                {
                    photonView.RPC(nameof(MainGameStartCall), RpcTarget.All);
                }
            }
        }

        [PunRPC]
        private void MainGameStartCall()
        {
            _isPlaying = true;
            DisableButtonExit();
            DisableButtonReady();
            Debug.Log("ゲームを始めるぞ！");
        }
    }
}
