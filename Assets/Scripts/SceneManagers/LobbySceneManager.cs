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
    public class LobbySceneManager : MonoBehaviour
    {
        private readonly Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();

        private void Start()
        {
            CanvasCreateRoom.Instance.Hide();
            CanvasEnterPassword.Instance.Hide();
            if (PhotonNetwork.IsConnected)
            {
                for (int i = 0; i < 30; i++)
                {
                    int ii = i;
                    _disposables.Add(
                        "RoomInteractable" + i,
                        RoomListModel.RoomsInteractable[i].Subscribe(interactable =>
                        {
                            CanvasRoomList.Instance.SetRoomButtonInteractable(ii, interactable);
                        }));

                    _disposables.Add(
                        "RoomStatus" + i,
                        RoomListModel.RoomStatuses[i].Subscribe(status =>
                        {
                            CanvasRoomList.Instance.SetRoomButtonStatus(ii, status);
                        }));

                    _disposables.Add(
                        "RoomName" + i,
                        RoomListModel.RoomNames[i].Subscribe(roomName =>
                        {
                            CanvasRoomList.Instance.SetRoomButtonRoomName(ii, roomName);
                            CanvasRoomList.Instance.SetRoomButtonRoomNameText(ii, roomName);
                        }));

                    _disposables.Add(
                        "RoomMaximum" + i,
                        RoomListModel.RoomMaximums[i].Subscribe(maximum =>
                        {
                            CanvasRoomList.Instance.SetRoomButtonMaximumText(ii, maximum);
                        }));

                    _disposables.Add(
                        "RoomCurrent" + i,
                        RoomListModel.RoomCurrents[i].Subscribe(current =>
                        {
                            CanvasRoomList.Instance.SetRoomButtonCurrentText(ii, current);
                        }));
                }

                _disposables.Add("RoomList", PhotonManager.Instance.RoomListUpdate.Subscribe(RoomListModel.Update));
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
                                PhotonNetwork.CreateRoom(CreateRoomModel.RoomName.Value, roomOptions);

                                _disposables.Add("OnJoinedRoom", PhotonManager.Instance.JoinedRoom.Subscribe(___ =>
                                    {
                                        SceneController.Instance.ChangeScene("MainScene");
                                    }
                                ));
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

        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables.Values) disposable.Dispose();
        }
        
        private void Logout()
        {
            _disposables.Add(
                "disconnected",
                PhotonManager.Instance.Disconnected.Subscribe(cause =>
                {
                    Dispose("disconnected");
                    SceneController.Instance.ChangeScene("TitleScene");
                }));
            
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
    }
}
