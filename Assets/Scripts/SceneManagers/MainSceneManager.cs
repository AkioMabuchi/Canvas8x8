using System;
using System.Collections;
using System.Collections.Generic;
using Canvases;
using Managers;
using Models;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
            CanvasForceHalt.Instance.Hide();
            CanvasCalls.Instance.HideImageCall();
            CanvasCalls.Instance.HideTextCall();
            CanvasPallet.Instance.Hide();
            CanvasAnswer.Instance.Hide();

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
            if (_isPlaying && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = true;
                photonView.RPC(nameof(MainGameForceFinish), RpcTarget.All);
            }
        }
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneController.Instance.ChangeScene("LobbyScene");
        }

        private void MainGameStart()
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
                    StartCoroutine(CoroutineGameStart());
                }
            }
        }

        private IEnumerator CoroutineGameStart()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            
            IReadOnlyList<Player> players = PlayerListModel.GetPlayerList();

            int[] examinerTurn = new int[players.Count];
            for (int i = 0; i < examinerTurn.Length; i++)
            {
                examinerTurn[i] = players[i].ActorNumber;
            }

            {
                int i = examinerTurn.Length - 1;
                while (i > 0)
                {
                    int j = UnityEngine.Random.Range(0, i);
                    int tmp = examinerTurn[i];
                    examinerTurn[i] = examinerTurn[j];
                    examinerTurn[j] = tmp;
                    i--;
                }
            }
            
            SetRoomProperty("Round", 0);
            SetRoomProperty("Examiners", examinerTurn);
            photonView.RPC(nameof(MainGameStartCall), RpcTarget.All);
            photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 0); // Game Startのサインを出す
            yield return new WaitForSeconds(3.0f);
            photonView.RPC(nameof(HideImageCall), RpcTarget.All);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CoroutineNextRound());
        }

        private IEnumerator CoroutineNextRound()
        {
            if ((int) PhotonNetwork.CurrentRoom.CustomProperties["Round"] < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                SetRoomProperty("Theme", ThemeModel.GetRandomTheme());
                photonView.RPC(nameof(UpdateTimer), RpcTarget.All, 60);
                photonView.RPC(nameof(NextRound), RpcTarget.All);
                photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 3);
                yield return new WaitForSeconds(1.0f);
                photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 2);
                yield return new WaitForSeconds(1.0f);
                photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 1);
                yield return new WaitForSeconds(1.0f);
                photonView.RPC(nameof(HideImageCall), RpcTarget.All);
                photonView.RPC(nameof(RoundStart), RpcTarget.All);
                for (int count = 60; count > 0; count--)
                {
                    photonView.RPC(nameof(UpdateTimer), RpcTarget.All, count);
                    yield return new WaitForSeconds(1.0f);
                }
                photonView.RPC(nameof(UpdateTimer), RpcTarget.All, 0);
                photonView.RPC(nameof(RoundEnd), RpcTarget.All);
                photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 5); // TimeUpのサインを出す
                yield return new WaitForSeconds(3.0f);
                photonView.RPC(nameof(HideImageCall), RpcTarget.All);
                yield return new WaitForSeconds(0.2f);
                
                int nextRound = (int) PhotonNetwork.CurrentRoom.CustomProperties["Round"] + 1;
                SetRoomProperty("Round", nextRound);
                StartCoroutine(CoroutineNextRound());
            }
            else
            {
                photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 6); // GameEndのサインを出す
                yield return new WaitForSeconds(3.0f);
                photonView.RPC(nameof(HideImageCall), RpcTarget.All);
                yield return new WaitForSeconds(0.2f);
                photonView.RPC(nameof(MainGameEndCall), RpcTarget.All);
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }

        private IEnumerator CoroutineAnswered()
        {
            photonView.RPC(nameof(RoundEnd), RpcTarget.All);
            photonView.RPC(nameof(ShowImageCall), RpcTarget.All, 4); // Answered!!のサインを出す
            yield return new WaitForSeconds(3.0f);
            photonView.RPC(nameof(HideImageCall), RpcTarget.All);
            yield return new WaitForSeconds(0.2f);
            
            int nextRound = (int) PhotonNetwork.CurrentRoom.CustomProperties["Round"] + 1;
            SetRoomProperty("Round", nextRound);
            StartCoroutine(CoroutineNextRound());
        }


        [PunRPC]
        private void ShowImageCall(int index)
        {
            CanvasCalls.Instance.ShowImageCall(index);
        }

        [PunRPC]
        private void HideImageCall()
        {
            CanvasCalls.Instance.HideImageCall();
        }

        [PunRPC]
        private void UpdateTimer(int count)
        {
            Debug.Log("のこり：" + count + "秒");
        }
        [PunRPC]
        private void MainGameStartCall()
        {
            _isPlaying = true;
            DisableButtonExit();
            DisableButtonReady();
            
            PictureModel.ClearCanvas();
            CanvasTheme.Instance.InitializeText();
        }

        [PunRPC]
        private void MainGameEndCall()
        {
            _isPlaying = false;
            _isReady = false;
            SetPlayerProperty("Status", PlayerStatus.NotReady);
            CanvasMain.Instance.ChangeButtonReadyImage(false);

            PictureModel.ClearCanvas();
            CanvasTheme.Instance.InitializeText();
        }

        [PunRPC]
        private void MainGameForceFinish()
        {
            StopAllCoroutines();
            
            _isPlaying = false;
            _isReady = false;
            SetPlayerProperty("Status", PlayerStatus.NotReady);
            CanvasMain.Instance.ChangeButtonReadyImage(false);
            
            _disposables.Add(
                "OnClickButtonErrorFormClose",
                CanvasForceHalt.Instance.OnClickButtonClose.Subscribe(_ =>
                {
                    Dispose("OnClickButtonErrorFormClose");
                    
                    CanvasForceHalt.Instance.Hide();

                    EnableButtonExit();
                    EnableButtonReady();
                }));
            
            PictureModel.ClearCanvas();
            CanvasTheme.Instance.InitializeText();
            CanvasForceHalt.Instance.Show();
        }

        [PunRPC]
        private void NextRound()
        {
            CanvasPallet.Instance.Hide();
            CanvasAnswer.Instance.Hide();
            
            int round = (int) PhotonNetwork.CurrentRoom.CustomProperties["Round"];

            int[] examiners = (int[]) PhotonNetwork.CurrentRoom.CustomProperties["Examiners"];

            if (examiners[round] == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PictureModel.ClearCanvas();
                PalletModel.ChangeColor(Color.white);
                CanvasPallet.Instance.Show();
                SetPlayerProperty("Status", PlayerStatus.Examiner);
                string answer = (string) PhotonNetwork.CurrentRoom.CustomProperties["Theme"];
                CanvasTheme.Instance.SetThemeText("お題：" + answer);
                CanvasCalls.Instance.ShowTextCall("あなたは「出題者」です");
            }
            else
            {
                CanvasAnswer.Instance.ChangeMode(CanvasAnswer.InputFieldMode.Disabled);
                CanvasAnswer.Instance.Show();
                SetPlayerProperty("Status", PlayerStatus.Answerer);
                CanvasTheme.Instance.SetThemeText("お題：？？？");
                CanvasCalls.Instance.ShowTextCall("あなたは「回答者」です");
            }
        }

        [PunRPC]
        private void RoundStart()
        {
            switch ((PlayerStatus) PhotonNetwork.LocalPlayer.CustomProperties["Status"])
            {
                case PlayerStatus.Examiner:
                {
                    _disposables.Add(
                        "OnClickImagePixel",
                        CanvasPicture.Instance.OnClickImagePixel.Subscribe(index =>
                        {
                            photonView.RPC(nameof(DrawPixel), RpcTarget.All, index, PalletModel.CurrentColor.Value);
                        }));
                    break;
                }
                case PlayerStatus.Answerer:
                {
                    _disposables.Add(
                        "AnswerText",
                        AnswerInputModel.InputText.Subscribe(text =>
                        {
                            CanvasAnswer.Instance.SetText(text);
                            CanvasAnswer.Instance.ChangeMode(ThemeModel.CanBeAnswer(text)
                                ? CanvasAnswer.InputFieldMode.Answerable
                                : CanvasAnswer.InputFieldMode.Enabled);
                        }));
                    
                    _disposables.Add(
                        "AnswerClicked",
                        CanvasAnswer.Instance.OnClickImageButtonAnswer.Subscribe(_ =>
                        {
                            string answer = AnswerInputModel.InputText.Value;

                            if (ThemeModel.CanBeAnswer(answer))
                            {
                                photonView.RPC(nameof(Answer), RpcTarget.MasterClient, answer);
                                SetPlayerProperty("Answer", answer);
                                AnswerInputModel.Clear();
                            }
                        }));

                    _disposables.Add(
                        "AnswerChar",
                        InputAnswerManager.Instance.OnInputKey.Subscribe(inputChar =>
                        {
                            switch (inputChar)
                            {
                                case '\r':
                                {
                                    string answer = AnswerInputModel.InputText.Value;

                                    if (ThemeModel.CanBeAnswer(answer))
                                    {
                                        photonView.RPC(nameof(Answer), RpcTarget.MasterClient, answer);
                                        SetPlayerProperty("Answer", answer);
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
                    AnswerInputModel.Clear();
                    break;
                }
            }
            CanvasCalls.Instance.HideTextCall();
        }

        [PunRPC]
        private void RoundEnd()
        {
            string answer = (string) PhotonNetwork.CurrentRoom.CustomProperties["Theme"];
            CanvasTheme.Instance.SetThemeText("お題：" + answer);
            switch ((PlayerStatus) PhotonNetwork.LocalPlayer.CustomProperties["Status"])
            {
                case PlayerStatus.Examiner:
                {
                    Dispose("OnClickImagePixel");
                    break;
                }
                case PlayerStatus.Answerer:
                {
                    Dispose("AnswerText");
                    Dispose("AnswerChar");
                    Dispose("AnswerClick");
                    CanvasAnswer.Instance.ChangeMode(CanvasAnswer.InputFieldMode.Disabled);
                    break;
                }
            }
        }
        
        [PunRPC]
        private void Answer(string answer) // このメソッドはマスタークライアントのみ実行していい
        {
            if ((string) PhotonNetwork.CurrentRoom.CustomProperties["Theme"] == ThemeModel.Answer(answer))
            {
                StopAllCoroutines();
                StartCoroutine(CoroutineAnswered());
            }
        }

        [PunRPC]
        private void DrawPixel(int index, Color color)
        {
            PictureModel.DrawPixel(index, color);
        }
    }
}
