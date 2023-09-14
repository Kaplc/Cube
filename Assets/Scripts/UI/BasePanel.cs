using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel<T> : MonoBehaviour where T: class
{

    private static T instance;
    public static T Instance => instance;
    
    private void Awake()
    {
        instance = this as T;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    protected abstract void Init();
    
    
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
