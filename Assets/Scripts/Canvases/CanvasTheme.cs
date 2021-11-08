using TMPro;
using UnityEngine;

namespace Canvases
{
    public class CanvasTheme : SingletonMonoBehaviour<CanvasTheme>
    {
        [SerializeField] private TextMeshProUGUI textMeshProTheme;

        public void SetThemeText(string text)
        {
            textMeshProTheme.text = text;
        }
    }
}
