using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Sounds
{
    public class SoundPlayer : SingletonMonoBehaviour<SoundPlayer>
    {
        [SerializeField] private SoundData soundData;
        [SerializeField] private GameObject prefab;

        private readonly Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private void Start()
        {
            foreach (SoundData.Relation relation in soundData.SoundList)
            {
                if (_audioClips.ContainsKey(relation.name))
                {
                    Debug.LogWarning("重複したキーがあります");
                    continue;
                }

                _audioClips.Add(relation.name, relation.sound);
            }
        }

        public void PlaySound(string soundName)
        {
            if (_audioClips.ContainsKey(soundName))
            {
                Instantiate(prefab, transform).GetComponent<Sound>().SetAudioClip(_audioClips[soundName]);
                return;
            }

            Debug.LogWarning("そのサウンド名は登録されていません");
        }
    }
}
