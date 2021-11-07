using TMPro;
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

        public void Show()
        {
            imageBackground.gameObject.SetActive(true);
        }

        public void Hide()
        {
            imageBackground.gameObject.SetActive(false);
        }
    }
}
