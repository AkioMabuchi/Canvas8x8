using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCalls : SingletonMonoBehaviour<CanvasCalls>
{
    [SerializeField] private Sprite[] spriteCalls = new Sprite[7];

    [SerializeField] private Image imageCall;
    [SerializeField] private TextMeshProUGUI textMeshProCall;

    public void ShowImageCall(int index)
    {
        imageCall.sprite = spriteCalls[index];
        imageCall.gameObject.SetActive(true);
    }

    public void HideImageCall()
    {
        imageCall.gameObject.SetActive(false);
    }

    public void ShowTextCall(string text)
    {
        textMeshProCall.text = text;
        textMeshProCall.gameObject.SetActive(true);
    }

    public void HideTextCall()
    {
        textMeshProCall.gameObject.SetActive(false);
    }
}