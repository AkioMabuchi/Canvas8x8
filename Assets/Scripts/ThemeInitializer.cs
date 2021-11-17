using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using ScriptableObjects;
using UnityEngine;

public class ThemeInitializer : MonoBehaviour
{
    [SerializeField] private ThemeData themeData;
    private void Start()
    {
        foreach (string theme in themeData.Themes)
        {
            ThemeModel.AddTheme(theme);
        }

        foreach (ThemeData.Relation relation in themeData.Relations)
        {
            ThemeModel.AddSynonym(relation.theme, relation.synonym);
        }
    }
}
