using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : SingletonMonoBehaviour<LoginForm>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_InputField inputFieldPlayerName;
    [SerializeField] private Button buttonLogin;
    [SerializeField] private TextMeshProUGUI textMeshProPlayerNameWarning;

    private readonly Subject<string> _onClickButtonLogin = new Subject<string>();
    public IObservable<string> OnClickButtonLogin => _onClickButtonLogin;
    private void Start()
    {
        canvasGroup.interactable = false;
        inputFieldPlayerName.text = "";
        textMeshProPlayerNameWarning.text = "";
        
        buttonLogin.onClick.AddListener(() =>
        {
            _onClickButtonLogin.OnNext(inputFieldPlayerName.text);
        });
    }

    public void SetPlayerNameWarningMessage(string text)
    {
        textMeshProPlayerNameWarning.text = "入力してください";
    }
    public void SetInteractable(bool interactable)
    {
        canvasGroup.interactable = interactable;
    }
}
