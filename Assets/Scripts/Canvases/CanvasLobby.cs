using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLobby : SingletonMonoBehaviour<CanvasLobby>
{
    [SerializeField] private Button buttonLogout;
    [SerializeField] private Button buttonCreateRoom;
        
    private readonly Subject<Unit> _onClickButtonLogout = new Subject<Unit>();
    public IObservable<Unit> OnClickButtonLogout => _onClickButtonLogout;
    private readonly Subject<Unit> _onClickButtonCreateRoom = new Subject<Unit>();
    public IObservable<Unit> OnClickButtonCreateRoom => _onClickButtonCreateRoom;
    private void Start()
    {
        buttonLogout.onClick.AddListener(() =>
        {
            _onClickButtonLogout.OnNext(Unit.Default);
        });
            
        buttonCreateRoom.onClick.AddListener(() =>
        {
            _onClickButtonCreateRoom.OnNext(Unit.Default);
        });
    }
}