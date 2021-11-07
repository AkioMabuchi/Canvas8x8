using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasEnterPassword : SingletonMonoBehaviour<CanvasEnterPassword>
    {        
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageForm;
        [SerializeField] private TMP_InputField inputFieldPassword;
        [SerializeField] private Button buttonEnter;
        [SerializeField] private Button buttonCancel;
        [SerializeField] private TextMeshProUGUI textMeshProPasswordWarning;

        private readonly Subject<string> _onCertificated = new Subject<string>();
        public IObservable<string> OnCertificated => _onCertificated;
        private readonly Subject<Unit> _onClickButtonCancel = new Subject<Unit>();
        public IObservable<Unit> OnClickButtonCancel => _onClickButtonCancel;
        
        private string _roomName;
        private string _roomPassword;

        private void Start()
        {
            buttonEnter.onClick.AddListener(() =>
            {
                if (_roomPassword == inputFieldPassword.text)
                {
                    textMeshProPasswordWarning.text = "";
                    _onCertificated.OnNext(_roomName);
                }
                else
                {
                    textMeshProPasswordWarning.text = "パスワードが違います";
                }
            });
        }

        public void SetRoomName(string roomName)
        {
            _roomName = roomName;
        }

        public void SetRoomPassword(string roomPassword)
        {
            _roomPassword = roomPassword;
        }
        public void Show()
        {
            textMeshProPasswordWarning.text = "";
            imageBackground.gameObject.SetActive(true);
        }

        public void Hide()
        {
            textMeshProPasswordWarning.text = "";
            imageBackground.gameObject.SetActive(false);
        }
    }
}
