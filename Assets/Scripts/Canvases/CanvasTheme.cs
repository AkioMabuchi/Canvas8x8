using TMPro;
using UnityEngine;

public class CanvasTheme : SingletonMonoBehaviour<CanvasTheme>
{
    [SerializeField] private TextMeshProUGUI textMeshProTheme;

    public void SetThemeText(string text)
    {
        textMeshProTheme.text = text;
    }

    public void InitializeText()
    {
        textMeshProTheme.text = "ここにお題が表示されます";
    }
}