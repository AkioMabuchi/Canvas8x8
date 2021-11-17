using System;
using Models;
using UniRx;
using UnityEngine;

namespace GameStates
{
    public class GameStateTitle : GameState
    {
        private IDisposable _disposable;
        public override void OnEnter()
        {
            _disposable = LoginForm.Instance.OnClickButtonLogin.Subscribe(playerName =>
            {
                if (playerName == "")
                {
                    LoginForm.Instance.SetPlayerNameWarningMessage("入力してください");
                }
                else
                {
                    LoginForm.Instance.SetPlayerNameWarningMessage("");
                    GameModel.ChangeStage(GameManager.State.Login);
                }
            });
            LoginForm.Instance.SetInteractable(true);
        }

        public override void OnExit()
        {
            _disposable.Dispose();
            LoginForm.Instance.SetInteractable(false);
        }
    }
}
