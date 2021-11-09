using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasTimer : SingletonMonoBehaviour<CanvasTimer>
{
    [SerializeField] private TextMeshProUGUI textMeshProCount;

    public void SetCountTextByInt(int count)
    {
        textMeshProCount.text = count.ToString();
    }
}