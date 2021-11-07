using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Canvases
{
    public class CanvasPicture : SingletonMonoBehaviour<CanvasPicture>
    {
        public void OnPointerDownImagePixel(int index)
        {
            Debug.Log(index);
        }
    }
}
