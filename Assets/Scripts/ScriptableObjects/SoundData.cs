using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObject/SoundData")]
    public class SoundData : ScriptableObject
    {
        [Serializable]
        public class Relation
        {
            public string name;
            public AudioClip sound;
        }

        [SerializeField] private List<Relation> soundList;
        public IReadOnlyList<Relation> SoundList => soundList;
    }
}
