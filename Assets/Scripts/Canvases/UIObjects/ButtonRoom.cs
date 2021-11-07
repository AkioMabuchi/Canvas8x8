using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases.UIObjects
{
    public class ButtonRoom : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textMeshProRoomName;
        [SerializeField] private TextMeshProUGUI textMeshProMaximum;
        [SerializeField] private TextMeshProUGUI textMeshProSlash;
        [SerializeField] private TextMeshProUGUI textMeshProCurrent;
        
        private string _roomName;
        
        private readonly Subject<string> _onClickButton = new Subject<string>();
        public IObservable<string> OnClickButton => _onClickButton;

        private void Reset()
        {
            button = GetComponent<Button>();
            textMeshProRoomName = transform.Find("TextMeshProRoomName").GetComponent<TextMeshProUGUI>();
            textMeshProMaximum = transform.Find("TextMeshProMaximum").GetComponent<TextMeshProUGUI>();
            textMeshProSlash = transform.Find("TextMeshProSlash").GetComponent<TextMeshProUGUI>();
            textMeshProCurrent = transform.Find("TextMeshProCurrent").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                _onClickButton.OnNext(_roomName);
            });
        }

        public void SetButtonImageSprite(Sprite sprite)
        {
            button.image.sprite = sprite;
        }
        
        public void ShowOrHideTexts(bool isShowing)
        {
            textMeshProRoomName.gameObject.SetActive(isShowing);
            textMeshProMaximum.gameObject.SetActive(isShowing);
            textMeshProSlash.gameObject.SetActive(isShowing);
            textMeshProCurrent.gameObject.SetActive(isShowing);
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
        
        public void SetRoomName(string roomName)
        {
            _roomName = roomName;
        }

        public void SetRoomNameText(string text)
        {
            textMeshProRoomName.text = text;
        }

        public void SetMaximumText(string text)
        {
            textMeshProMaximum.text = text;
        }

        public void SetCurrentText(string text)
        {
            textMeshProCurrent.text = text;
        }
    }
}
