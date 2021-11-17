using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BlackCircle : SingletonMonoBehaviour<BlackCircle>
{
    [SerializeField] private Image imageMask;
    [SerializeField] private Image imageUnmask;
    
    private void Start()
    {
        imageUnmask.transform.localScale = Vector3.forward;
    }

    public void ShowUp()
    {
        imageUnmask.transform.DOScale(Vector3.one, 2.0f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            imageMask.gameObject.SetActive(false);
        });
    }
}

