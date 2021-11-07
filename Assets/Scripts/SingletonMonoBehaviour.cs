using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Null Instance");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            OnAwake();
            return;
        }

        if (_instance == this)
        {
            OnAwake();
            return;
        }
        
        Destroy(this);
    }

    private void OnDestroy()
    {
        OnRelease();
        Destroy(this);
    }

    protected virtual void OnAwake()
    {
        
    }

    protected virtual void OnRelease()
    {
        
    }
}
