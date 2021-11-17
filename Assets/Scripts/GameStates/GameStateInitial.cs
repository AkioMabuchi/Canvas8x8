using System;
using Models;
using UniRx;

namespace GameStates
{
    public class GameStateInitial : GameState
    {
        public override void OnEnter()
        {
            BlackCircle.Instance.ShowUp();
            Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(_ =>
            {
                GameModel.ChangeStage(GameManager.State.Title);
            });
        }
    }
}
