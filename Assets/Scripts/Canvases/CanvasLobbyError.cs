using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLobbyError : SingletonMonoBehaviour<CanvasLobbyError>
{
    [SerializeField] private Image imageBackground;
    [SerializeField] private Image imageFrame;
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonAccept;
    [SerializeField] private TextMeshProUGUI textMeshProMessage;
        
    private readonly Subject<Unit> _onClickButtonClose = new Subject<Unit>();
    public IObservable<Unit> OnClickButtonClose => _onClickButtonClose;
    private void Start()
    {
        buttonCancel.onClick.AddListener(() =>
        {
            _onClickButtonClose.OnNext(Unit.Default);
        });
            
        buttonAccept.onClick.AddListener(() =>
        {
            _onClickButtonClose.OnNext(Unit.Default);
        });
    }

    public void Show()
    {
        imageBackground.gameObject.SetActive(true);
    }

    public void Hide()
    {
        imageBackground.gameObject.SetActive(false);
    }

    public void SetMessageText(string text)
    {
        textMeshProMessage.text = text;
    }
}