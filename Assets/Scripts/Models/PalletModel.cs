using UniRx;
using UnityEngine;

namespace Models
{
    public static class PalletModel
    {
        private static readonly ReactiveProperty<Color>[] _palletColors = new ReactiveProperty<Color>[20];
        private static readonly ReactiveProperty<Color> _currentColor = new ReactiveProperty<Color>(Color.white);
        public static IReadOnlyReactiveProperty<Color> CurrentColor => _currentColor;

        static PalletModel()
        {
            for (int i = 0; i < 20; i++)
            {
                _palletColors[i] = new ReactiveProperty<Color>();
            }
        }

        public static void SetPalletColor(int index, Color color)
        {
            _palletColors[index].Value = color;
        }

        public static void ChangeColor(int index)
        {
            _currentColor.Value = _palletColors[index].Value;
        }

        public static void ChangeColor(Color color)
        {
            _currentColor.Value = color;
        }
    }
}
