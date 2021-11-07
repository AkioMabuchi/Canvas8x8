using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasTitle : SingletonMonoBehaviour<CanvasTitle>
    {
        [SerializeField] private TMP_InputField inputFieldUserName;
        [SerializeField] private Button buttonLogin;
        
        private readonly Subject<string> _onChangeInputFieldUserName = new Subject<string>();
        public IObservable<string> OnChangeInputFieldUserName => _onChangeInputFieldUserName;
        
        private readonly Subject<Unit> _onClickButtonLogin = new Subject<Unit>();
        public IObservable<Unit> OnClickButtonLogin => _onClickButtonLogin;
        private void Start()
        {
            inputFieldUserName.onDeselect.AddListener(userName =>
            {
                _onChangeInputFieldUserName.OnNext(userName);
            });
            
            buttonLogin.onClick.AddListener(() =>
            {
                _onClickButtonLogin.OnNext(Unit.Default);
            });
        }

        public void SetInputFieldUserNameText(string text)
        {
            inputFieldUserName.text = text;
        }
        
        public void SetInputFieldUserNameInteractable(bool isInteractable)
        {
            inputFieldUserName.interactable = isInteractable;
        }

        public void SetButtonLoginInteractable(bool isInteractable)
        {
            buttonLogin.interactable = isInteractable;
        }
    }
}
