using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 公共Mono管理器
/// </summary>
public class MonoManagement : MonoBehaviour
{
    public static MonoManagement instance;
    private event UnityAction updateEvent;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }
    public void Coroutine(IEnumerator routine)
    {
          StartCoroutine(routine);
    }



    /*
    void Update()
    {
        if (updateEvent!=null)
        {
            updateEvent.Invoke();
        }
    }
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    public void RemoveUpdateListenter(UnityAction fun)
    {
        updateEvent -= fun;
    }
    */
}
