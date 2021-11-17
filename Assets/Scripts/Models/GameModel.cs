using GameStates;
using ScriptableObjects;
using UniRx;
using UnityEngine;

namespace Models
{
    public static class GameModel
    {
        private static readonly ReactiveProperty<GameManager.State> _state =
            new ReactiveProperty<GameManager.State>(GameManager.State.Initial);
        public static IReadOnlyReactiveProperty<GameManager.State> State => _state;

        public static void ChangeStage(GameManager.State state)
        {
            _state.Value = state;
        }
    }
}
