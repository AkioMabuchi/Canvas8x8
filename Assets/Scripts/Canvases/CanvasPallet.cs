using System;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasPallet : SingletonMonoBehaviour<CanvasPallet>
    {
        [SerializeField] private Image imageFrame;

        public void Show()
        {
            imageFrame.gameObject.SetActive(true);
        }

        public void Hide()
        {
            imageFrame.gameObject.SetActive(false);
        }

        public void OnPointerDownImageButtonColor(int index)
        {
            
        }
    }
}
