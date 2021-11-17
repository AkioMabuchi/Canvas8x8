using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSlide : SingletonMonoBehaviour<MainSlide>
{
    private void Start()
    {
        transform.localPosition = new Vector3(2240.0f, 0.0f, 0.0f);
    }
}
