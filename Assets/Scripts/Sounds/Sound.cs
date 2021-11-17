using System;
using System.Collections;
using UnityEngine;

namespace Sounds
{
    public class Sound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        private AudioClip _audioClip;


        private IEnumerator Start()
        {
            audioSource.clip = _audioClip;
            audioSource.Play();
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            _audioClip = audioClip;
        }
    }
}
