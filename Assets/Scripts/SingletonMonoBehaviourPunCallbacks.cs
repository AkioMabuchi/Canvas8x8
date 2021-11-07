using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class SingletonMonoBehaviourPunCallbacks<T> : MonoBehaviourPunCallbacks
    where T : SingletonMonoBehaviourPunCallbacks<T>
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
