using System;
using Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCreateRoom : SingletonMonoBehaviour<CanvasCreateRoom>
{
    [SerializeField] private Image imageBackground;
    [SerializeField] private Image imageForm;
    [SerializeField] private TMP_InputField inputFieldRoomName;
    [SerializeField] private TMP_InputField inputFieldRoomPassword;
    [SerializeField] private Button buttonCreate;
    [SerializeField] private Button buttonCancel;
    [SerializeField] private TextMeshProUGUI textMeshProRoomNameWarning;

    private readonly Subject<string> _onChangeInputFieldRoomName = new Subject<string>();
    private readonly Subject<string> _onChangeInputFieldRoomPassword = new Subject<string>();

    private readonly Subject<Unit> _onClickButtonCreate = new Subject<Unit>();
    public IObservable<Unit> OnClickButtonCreate => _onClickButtonCreate;
    private readonly Subject<Unit> _onClickButtonCancel = new Subject<Unit>();
    public IObservable<Unit> OnClickButtonCancel => _onClickButtonCancel;
    protected override void OnAwake()
    {
        CreateRoomModel.RoomName.Subscribe(roomName =>
        {
            inputFieldRoomName.text = roomName;
        }).AddTo(gameObject);

        CreateRoomModel.RoomPassword.Subscribe(roomPassword =>
        {
            inputFieldRoomPassword.text = roomPassword;
        }).AddTo(gameObject);

        _onChangeInputFieldRoomName.Subscribe(CreateRoomModel.SetRoomName).AddTo(gameObject);
        _onChangeInputFieldRoomPassword.Subscribe(CreateRoomModel.SetRoomPassword).AddTo(gameObject);
    }

    private void Start()
    {
        inputFieldRoomName.onDeselect.AddListener(roomName =>
        {
            _onChangeInputFieldRoomName.OnNext(roomName);
        });
            
        inputFieldRoomPassword.onDeselect.AddListener(roomPassword =>
        {
            _onChangeInputFieldRoomPassword.OnNext(roomPassword);
        });
            
        buttonCreate.onClick.AddListener(() =>
        {
            _onClickButtonCreate.OnNext(Unit.Default);
        });
            
        buttonCancel.onClick.AddListener(() =>
        {
            _onClickButtonCancel.OnNext(Unit.Default);
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

    public  void SetRoomNameWarningText(string text)
    {
        textMeshProRoomNameWarning.text = text;
    }


}