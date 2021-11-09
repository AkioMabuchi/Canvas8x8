using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Models;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace SceneManagers
{
    public class TitleSceneManager : MonoBehaviourPunCallbacks
    {
        private readonly Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();

        private IEnumerator Start()
        {
            SceneController.Instance.SetCurrentSceneName("TitleScene");
            _disposables.Add(
                "UserName",
                UserNameModel.UserName.Subscribe(userName =>
                {
                    CanvasTitle.Instance.SetInputFieldUserNameText(userName);
                }));

            CanvasTitle.Instance.SetInputFieldUserNameText(UserNameModel.UserName.Value);
            CanvasTitle.Instance.SetInputFieldUserNameInteractable(false);
            CanvasTitle.Instance.SetButtonLoginInteractable(false);

            CanvasTitle.Instance.SetWarningText("");
            CanvasTitleConnecting.Instance.Hide();
            CanvasTitleError.Instance.Hide();
            
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
                while (PhotonNetwork.IsConnected) yield return null;
            }

            EnableUserControl();
        }

        private void EnableUserControl()
        {
            _disposables.Add(
                "OnChangeInputFieldUserName",
                CanvasTitle.Instance.OnChangeInputFieldUserName.Subscribe(UserNameModel.SetUserName));
            
            _disposables.Add(
                "OnClickButtonLogin",
                CanvasTitle.Instance.OnClickButtonLogin.Subscribe(_ =>
                {
                    if (UserNameModel.UserName.Value == "")
                    {
                        CanvasTitle.Instance.SetWarningText("入力してください");
                        return;
                    }

                    CanvasTitle.Instance.SetWarningText("");
                    DisableUserControl();
                    ConnectServerAndJoinLobby();

                }));
            
            CanvasTitle.Instance.SetInputFieldUserNameInteractable(true);
            CanvasTitle.Instance.SetButtonLoginInteractable(true);
        }

        private void DisableUserControl()
        {
            Dispose("OnChangeInputFieldUserName");
            Dispose("OnClickButtonLogin");
            CanvasTitle.Instance.SetInputFieldUserNameInteractable(false);
            CanvasTitle.Instance.SetButtonLoginInteractable(false);
        }

        private void ConnectServerAndJoinLobby()
        {
            CanvasTitleConnecting.Instance.Show();
            PhotonNetwork.ConnectUsingSettings();
        }
        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables.Values) disposable.Dispose();
        }
        
        private void Dispose(string key)
        {
            if (_disposables.ContainsKey(key))
            {
                _disposables[key].Dispose();
                return;
            }

            Debug.LogWarning(key);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneController.Instance.ChangeScene("LobbyScene");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            switch (cause)
            {
                case DisconnectCause.MaxCcuReached:
                {
                    CanvasTitleError.Instance.SetMessageText("サーバーが満員のため、接続できませんでした");
                    ShowErrorDialog();
                    break;
                }
            }
        }

        private void ShowErrorDialog()
        {
            _disposables.Add(
                "ButtonErrorCancel",
                CanvasTitleError.Instance.OnClickButtonClose.Subscribe(_ =>
                {
                    Dispose("ButtonErrorCancel");
                    CanvasTitleError.Instance.Hide();
                    EnableUserControl();
                }));
            CanvasTitleConnecting.Instance.Hide();
            CanvasTitleError.Instance.Show();
        }
    }
}
