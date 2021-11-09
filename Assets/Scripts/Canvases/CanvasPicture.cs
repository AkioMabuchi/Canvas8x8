using System;
using Models;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasPicture : SingletonMonoBehaviour<CanvasPicture>
{
    [SerializeField] private Image[] imagesPixel = new Image[64];

    private readonly Subject<int> _onClickImagePixel = new Subject<int>();
    public IObservable<int> OnClickImagePixel => _onClickImagePixel;
    protected override void OnAwake()
    {
        for (int i = 0; i < 64; i++)
        {
            int ii = i;
            PictureModel.PixelsColor[i].Subscribe(color =>
            {
                imagesPixel[ii].color = color;
            }).AddTo(gameObject);
        }
    }

    public void OnPointerDownImagePixel(int index)
    {
        _onClickImagePixel.OnNext(index);
    }
}