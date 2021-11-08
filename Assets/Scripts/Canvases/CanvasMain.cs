using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasMain : SingletonMonoBehaviour<CanvasMain>
    {
        [SerializeField] private Sprite spriteButtonReady;

        [SerializeField] private Sprite spriteButtonReady2;

        [SerializeField] private Button buttonExit;

        [SerializeField] private Button buttonReady;

        private readonly Subject<Unit> _onClickButtonExit = new Subject<Unit>();
        public IObservable<Unit> OnClickButtonExit => _onClickButtonExit;
        
        private readonly Subject<Unit> _onClickButtonReady = new Subject<Unit>();
        public IObservable<Unit> OnClickButtonReady => _onClickButtonReady;
        private void Start()
        {
            buttonExit.onClick.AddListener(() =>
            {
                _onClickButtonExit.OnNext(Unit.Default);
            });
            
            buttonReady.onClick.AddListener(() =>
            {
                _onClickButtonReady.OnNext(Unit.Default);
            });
        }

        public void SetButtonExitInteractable(bool interactable)
        {
            buttonExit.interactable = interactable;
        }

        public void SetButtonReadyInteractable(bool interactable)
        {
            buttonReady.interactable = interactable;
        }

        public void ChangeButtonReadyImage(bool s)
        {
            buttonReady.image.sprite = s ? spriteButtonReady2 : spriteButtonReady;
        }
    }
}
