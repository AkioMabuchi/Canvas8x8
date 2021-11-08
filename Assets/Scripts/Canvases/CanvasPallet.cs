using System;
using Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasPallet : SingletonMonoBehaviour<CanvasPallet>
    {
        [SerializeField] private Image imageFrame;

        [SerializeField] private Image imageCurrentColor;
        [SerializeField] private Image[] imagesColors = new Image[20];

        private readonly Subject<int> _onClickImageButtonColor = new Subject<int>();
        protected override void OnAwake()
        {
            PalletModel.CurrentColor.Subscribe(color =>
            {
                imageCurrentColor.color = color;
            }).AddTo(gameObject);

            _onClickImageButtonColor.Subscribe(PalletModel.ChangeColor).AddTo(gameObject);
            
            for (int i = 0; i < 20; i++)
            {
                PalletModel.SetPalletColor(i, imagesColors[i].color);
            }
        }

        public void Show()
        {
            imageFrame.gameObject.SetActive(true);
        }

        public void Hide()
        {
            imageFrame.gameObject.SetActive(false);
        }

        public void OnPointerDownImageButtonColor(int index)
        {
            _onClickImageButtonColor.OnNext(index);
        }
    }
}
