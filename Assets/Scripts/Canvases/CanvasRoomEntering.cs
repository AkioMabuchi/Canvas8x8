using UnityEngine;
using UnityEngine.UI;

public class CanvasRoomEntering : SingletonMonoBehaviour<CanvasRoomEntering>
{
    [SerializeField] private Image imageBackground;
        
    public void Show()
    {
        imageBackground.gameObject.SetActive(true);
    }

    public void Hide()
    {
        imageBackground.gameObject.SetActive(false);
    }
}