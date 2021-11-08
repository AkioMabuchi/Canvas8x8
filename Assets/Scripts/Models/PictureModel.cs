using UniRx;
using UnityEngine;

namespace Models
{
    public static class PictureModel
    {
        private static readonly ReactiveProperty<Color>[] _pixelsColor = new ReactiveProperty<Color>[64];

        public static readonly IReadOnlyReactiveProperty<Color>[]
            PixelsColor = new IReadOnlyReactiveProperty<Color>[64];

        static PictureModel()
        {
            for (int i = 0; i < 64; i++)
            {
                _pixelsColor[i] = new ReactiveProperty<Color>(Color.white);
                PixelsColor[i] = _pixelsColor[i];
            }
        }

        public static void DrawPixel(int index, Color color)
        {
            _pixelsColor[index].Value = color;
        }

        public static void ClearCanvas()
        {
            for (int i = 0; i < 64; i++)
            {
                _pixelsColor[i].Value = Color.white;
            }
        }
    }
}
