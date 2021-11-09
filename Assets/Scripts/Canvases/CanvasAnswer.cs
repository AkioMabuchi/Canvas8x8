using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnswer : SingletonMonoBehaviour<CanvasAnswer>
{
    public enum InputFieldMode
    {
        Disabled,
        Enabled,
        Answerable
    }

    [SerializeField] private Sprite spriteAnswerDisabled;
    [SerializeField] private Sprite spriteAnswerEnabled;
        
    [SerializeField] private Image imageFrame;
    [SerializeField] private Image imageInputField;
    [SerializeField] private TextMeshProUGUI textMeshProInputText;
    [SerializeField] private Image imageCaret;
    [SerializeField] private Image imageButtonAnswer;
        
    private readonly Subject<Unit> _onClickImageButtonAnswer = new Subject<Unit>();
    public IObservable<Unit> OnClickImageButtonAnswer => _onClickImageButtonAnswer;

    public void Show()
    {
        imageFrame.gameObject.SetActive(true);
    }

    public void Hide()
    {
        imageFrame.gameObject.SetActive(false);
    }

    public void ChangeMode(InputFieldMode mode)
    {
        imageInputField.color = mode switch
        {
            InputFieldMode.Disabled => new Color(0.8f, 0.8f, 0.8f),
            InputFieldMode.Enabled => new Color(1.0f, 1.0f, 1.0f),
            InputFieldMode.Answerable => new Color(1.0f, 1.0f, 0.7f),
            _ => imageInputField.color
        };

        imageButtonAnswer.sprite = mode == InputFieldMode.Answerable ? spriteAnswerEnabled : spriteAnswerDisabled;
        imageCaret.gameObject.SetActive(mode != InputFieldMode.Disabled);
    }
    public void SetText(string text)
    {
        textMeshProInputText.text = text;
        float caratPositionX = textMeshProInputText.preferredWidth - 270.0f;
        imageCaret.transform.localPosition = new Vector3(caratPositionX, 0.0f, 0.0f);
    }

    public void OnPointerDownImageButtonAnswer()
    {
        _onClickImageButtonAnswer.OnNext(Unit.Default);
    }
}