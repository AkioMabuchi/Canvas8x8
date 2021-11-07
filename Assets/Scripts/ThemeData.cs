using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ThemeData")]
public class ThemeData : ScriptableObject
{
    [Serializable]
    public class Relation
    {
        public string theme;
        public string synonym;
    }
    
    [SerializeField] private List<string> themes;
    public IReadOnlyList<string> Themes => themes;

    [SerializeField] private List<Relation> relations;
    public IReadOnlyList<Relation> Relations => relations;
}
