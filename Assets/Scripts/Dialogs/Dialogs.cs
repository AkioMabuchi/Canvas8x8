using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogs
{
    public class Dialogs : SingletonMonoBehaviour<Dialogs>
    {
        [SerializeField] private Image imageBackground;

        private void Start()
        {
            imageBackground.gameObject.SetActive(false);
        }

        public void Show()
        {
            imageBackground.gameObject.SetActive(true);
        }

        public void Hide()
        {
            imageBackground.gameObject.SetActive(false);
        }
    }
}
