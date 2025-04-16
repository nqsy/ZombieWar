using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            throw new Exception($"there are more than 1 instance of type {typeof(T).Name}");
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
