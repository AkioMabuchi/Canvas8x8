using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialogs
{
    public class DialogConnectingServer : SingletonMonoBehaviour<DialogConnectingServer>
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;

        private bool _isShowing;
        private void Start()
        {
            textMeshPro.gameObject.SetActive(false);
        }

        public void Show()
        {
            textMeshPro.gameObject.SetActive(true);
            _isShowing = true;
            StartCoroutine(Coroutine());
        }

        public void Hide()
        {
            textMeshPro.gameObject.SetActive(false);
            _isShowing = false;
        }

        private IEnumerator Coroutine()
        {
            while (_isShowing)
            {
                textMeshPro.text = "サーバーに接続中";
                yield return new WaitForSeconds(0.5f);
                textMeshPro.text = "サーバーに接続中・";
                yield return new WaitForSeconds(0.5f);
                textMeshPro.text = "サーバーに接続中・・";
                yield return new WaitForSeconds(0.5f);
                textMeshPro.text = "サーバーに接続中・・・";
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
