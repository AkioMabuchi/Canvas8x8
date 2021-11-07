using TMPro;
using UnityEngine;

namespace Canvases
{
    public class CanvasAnswer : SingletonMonoBehaviour<CanvasAnswer>
    {
        [SerializeField] private TextMeshProUGUI textMeshProInputText;
        
        public void SetText(string text)
        {
            textMeshProInputText.text = text;
        }
    }
}
