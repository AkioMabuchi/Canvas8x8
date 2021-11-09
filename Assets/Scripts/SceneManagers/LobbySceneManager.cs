using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Managers;
using Models;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace SceneManagers
{
    public class LobbySceneManager : MonoBehaviourPunCallbacks
    {
        private readonly Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();

        private void Start()
        {
            SceneController.Instance.SetCurrentSceneName("LobbyScene");
            CanvasCreateRoom.Instance.Hide();
            CanvasEnterPassword.Instance.Hide();
            CanvasRoomEntering.Instance.Hide();
            CanvasLobbyError.Instance.Hide();
            
            if (PhotonNetwork.InLobby)
            {
                EnableUserControl();
            }
            else
            {
                SceneController.Instance.ChangeScene("TitleScene");
            }
        }

        private void EnableUserControl()
        {
            _disposables.Add(
                "OnClickButtonLogout",
                CanvasLobby.Instance.OnClickButtonLogout.Subscribe(_ =>
                {
                    DisableUserControl();
                    Logout();
                }));

            _disposables.Add(
                "OnClickButtonCreateRoom",
                CanvasLobby.Instance.OnClickButtonCreateRoom.Subscribe(_ =>
                {
                    _disposables.Add(
                        "OnClickButtonCreate",
                        CanvasCreateRoom.Instance.OnClickButtonCreate.Subscribe(__ =>
                        {
                            if (CreateRoomModel.RoomName.Value == "")
                            {
                                CanvasCreateRoom.Instance.SetRoomNameWarningText("ルーム名を入力してください");
                            }
                            else if (RoomListModel.HasRoom(CreateRoomModel.RoomName.Value))
                            {
                                CanvasCreateRoom.Instance.SetRoomNameWarningText("そのルーム名は既に使われています");
                            }
                            else
                            {
                                DisableCreateRoomControlAndHide();
                                RoomOptions roomOptions = new RoomOptions
                                {
                                    MaxPlayers = 5,
                                    CustomRoomProperties = new Hashtable
                                    {
                                        {
                                            "Password", CreateRoomModel.RoomPassword.Value
                                        }
                                    },
                                    CustomRoomPropertiesForLobby = new[] {"Password"}
                                };
                                
                                CanvasRoomEntering.Instance.Show();
                                PhotonNetwork.CreateRoom(CreateRoomModel.RoomName.Value, roomOptions);
                            }
                        }));
                    _disposables.Add(
                        "OnClickButtonCreateRoomCancel",
                        CanvasCreateRoom.Instance.OnClickButtonCancel.Subscribe(__ =>
                        {
                            DisableCreateRoomControlAndHide();
                            EnableUserControl();
                        }));
                    DisableUserControl();
                    CreateRoomModel.SetRoomName("");
                    CreateRoomModel.SetRoomPassword("");
                    CanvasCreateRoom.Instance.SetRoomNameWarningText("");
                    CanvasCreateRoom.Instance.Show();
                }));

            _disposables.Add(
                "OnClickButtonRoom",
                CanvasRoomList.Instance.OnClickButtonRoom.Subscribe(roomName =>
                {
                    if (RoomListModel.Rooms.ContainsKey(roomName))
                    {
                        string password = (string) RoomListModel.Rooms[roomName].CustomProperties["Password"];
                        if (password == "")
                        {
                            PhotonNetwork.JoinRoom(roomName);
                        }
                        else
                        {
                            _disposables.Add(
                                "OnCertificated",
                                CanvasEnterPassword.Instance.OnCertificated.Subscribe(roomName2 =>
                                {
                                    DisableEnterPasswordControlAndHide();
                                    CanvasRoomEntering.Instance.Show();
                                    PhotonNetwork.JoinRoom(roomName2);
                                }));
                            _disposables.Add(
                                "OnClickButtonEnterPasswordCancel",
                                CanvasEnterPassword.Instance.OnClickButtonCancel.Subscribe(_ =>
                                {
                                    DisableEnterPasswordControlAndHide();
                                    EnableUserControl();
                                }));
                            CanvasEnterPassword.Instance.SetRoomName(roomName);
                            CanvasEnterPassword.Instance.SetRoomPassword(password);
                            CanvasEnterPassword.Instance.Show();
                        }
                        DisableUserControl();
                    }
                    else
                    {
                        Debug.Log("This Room Deleted");
                    }
                }));
        }

        private void DisableUserControl()
        {
            Dispose("OnClickButtonLogout");
            Dispose("OnClickButtonCreateRoom");
            Dispose("OnClickButtonRoom");
        }
        
        private void DisableCreateRoomControlAndHide()
        {
            Dispose("OnClickButtonCreate");
            Dispose("OnClickButtonCreateRoomCancel");
            CanvasCreateRoom.Instance.Hide();
        }

        private void DisableEnterPasswordControlAndHide()
        {
            Dispose("OnCertificated");
            Dispose("OnClickButtonEnterPasswordCancel");
            CanvasEnterPassword.Instance.Hide();
        }
        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables.Values) disposable.Dispose();
        }
        
        private void Logout()
        {
            PhotonNetwork.Disconnect();
        }

        private void Dispose(string key)
        {
            if (_disposables.ContainsKey(key))
            {
                _disposables[key].Dispose();
                _disposables.Remove(key);
            }
        }

        public override void OnJoinedRoom()
        {
            SceneController.Instance.ChangeScene("MainScene");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            CanvasLobbyError.Instance.SetMessageText("入室に失敗しました");
            ShowErrorDialog();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneController.Instance.ChangeScene("TitleScene");
        }

        private void ShowErrorDialog()
        {
            _disposables.Add(
                "ButtonErrorCancel",
                CanvasLobbyError.Instance.OnClickButtonClose.Subscribe(_ =>
                {
                    Dispose("ButtonErrorCancel");
                    CanvasLobbyError.Instance.Hide();
                    EnableUserControl();
                }));
            CanvasRoomEntering.Instance.Hide();
            CanvasLobbyError.Instance.Show();
        }
    }
}
