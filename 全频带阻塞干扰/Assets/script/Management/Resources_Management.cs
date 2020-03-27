using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源加载管理
/// </summary>
public class Resources_Management:InstanceNull<Resources_Management>
{
    public void Load<T>(string path, UnityAction<T> acllback, Transform createsite = null) where T:Object//同步加载
    {
        T res= Resources.Load<T>(path);
        if (res is GameObject)
        {
            acllback(GameObject.Instantiate(res, createsite) as T);
        }
        else
        {
            acllback(res as T);
        }
    }
    
    public void LoadAsync<T>(string path, UnityAction<T> acllback, Transform createsite = null) where T : Object//异步加载
    {
        MonoManagement.instance.Coroutine(RellyLoadAsync(path, acllback, createsite));
    }
   

    private IEnumerator RellyLoadAsync<T>(string path,  UnityAction<T> acllback, Transform createsite = null) where T : Object
    {
        ResourceRequest res = Resources.LoadAsync<T>(path);
        yield return res;
        if (res.asset is GameObject)
        {
            acllback(GameObject.Instantiate(res.asset, createsite) as T);
        }
        else
        {
            acllback(res.asset as T);
        }
    }
}
