using System;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class CanvasPallet : SingletonMonoBehaviour<CanvasPallet>
    {
        [SerializeField] private Image imageFrame;

        [SerializeField] private Transform transformColors;
        [SerializeField] private GameObject prefab;

        private void Start()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject gameObjectColor = Instantiate(prefab, transformColors);
                gameObjectColor.name = "ImageColor (" + i + ")";
                float positionX = i % 10 * 60.0f;
                float positionY = i / 10 * 60.0f;
                gameObjectColor.transform.localPosition = new Vector3(positionX, positionY, 0.0f);
            }
        }

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
