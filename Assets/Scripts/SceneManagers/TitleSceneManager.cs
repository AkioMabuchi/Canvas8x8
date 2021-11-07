using System;
using System.Collections;
using System.Collections.Generic;
using Canvases;
using Cysharp.Threading.Tasks;
using Managers;
using Models;
using Photon.Pun;
using SceneManagers.TitleSceneStates;
using UniRx;
using UnityEngine;

namespace SceneManagers
{
    public class TitleSceneManager : MonoBehaviour
    {
        private readonly Dictionary<string, IDisposable> _disposables = new Dictionary<string, IDisposable>();

        private IEnumerator Start()
        {
            _disposables.Add(
                "UserName",
                UserNameModel.UserName.Subscribe(userName =>
                {
                    CanvasTitle.Instance.SetInputFieldUserNameText(userName);
                }));

            CanvasTitle.Instance.SetInputFieldUserNameText(UserNameModel.UserName.Value);
            CanvasTitle.Instance.SetInputFieldUserNameInteractable(false);
            CanvasTitle.Instance.SetButtonLoginInteractable(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
                yield return UniTask.WaitUntil(() => !PhotonNetwork.IsConnected);
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
                        return;
                    }
                        
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
            _disposables.Add("OnConnectedToMaster",PhotonManager.Instance.ConnectedToMaster.Subscribe(_ =>
            {
                Dispose("OnConnectedToMaster");
                _disposables.Add("OnJoinedLobby",PhotonManager.Instance.JoinedLobby.Subscribe(__ =>
                {
                    Dispose("OnJoinedLobby");
                    SceneController.Instance.ChangeScene("LobbyScene");
                }));
                PhotonNetwork.JoinLobby();
            }));
            
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
    }
}
