using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 管理场景中的所有已生成的UI
/// </summary>
public class UI_Management : MonoBehaviour
{
    public static UI_Management instance;
    public Dictionary<string, GameObject> UIdict = new Dictionary<string, GameObject>();
    void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        FindChildrenControl<Button>();
    }
    public T GetControl<T>(string controlName) where T : UIBehaviour//获取UI对象
    {
        if (UIdict.ContainsKey(controlName))
        {
            if (UIdict[controlName].GetComponent<T>())
            {
                return UIdict[controlName].GetComponent<T>();
            }
            else
            {
                UIdict[controlName].AddComponent<T>();
                return UIdict[controlName].GetComponent<T>();
            }
        }
        return null;
    }
    /// <summary>
    /// 通过key添加按钮点击事件
    /// </summary>
    /// <typeparam name="T">UI类型</typeparam>
    /// <param name="controlName">UI名字</param>
    /// <param name="callBack">回调函数(逻辑)</param>    
    /// /// <param name="audioName">音效名字</param>
    /// <param name="callBackAudio">回调函数(音效)</param>
    public void AddButtonEventTrigger<T>(string controlName, UnityAction callBack=null,string audioName=null, UnityAction<string> callBackAudio=null) where T: Button
    {
        if (! UIdict.ContainsKey(controlName))
        {
            return;
        }
        if (UIdict[controlName])
        {
            EventTrigger trigger;
            if (UIdict[controlName].GetComponent<EventTrigger>() != null)
            {
                trigger = UIdict[controlName].GetComponent<EventTrigger>();
            }
            else
            {
                trigger = UIdict[controlName].AddComponent<EventTrigger>();
            }

            if (callBack!=null)
            {
                EventTrigger.Entry onclick = new EventTrigger.Entry();
                onclick.eventID = EventTriggerType.PointerUp;
                onclick.callback.AddListener((BaseEventData value) => { callBack(); });
                trigger.triggers.Add(onclick);
            }
            if (callBackAudio!=null)
            {
                EventTrigger.Entry upclick = new EventTrigger.Entry();
                upclick.eventID = EventTriggerType.PointerDown;
                upclick.callback.AddListener((BaseEventData value) => {callBackAudio(audioName);});
                trigger.triggers.Add(upclick);
            }
        }
    }

    /// <summary>
    /// 通过GameObject添加按钮点击事件
    /// </summary>
    /// <param name="obj">UI对象</param>
    /// <param name="callBack">回调函数(逻辑)</param>
    /// <param name="audioName">音效名字</param>
    /// <param name="callBackAudio">回调函数(音效)</param>
    public void AddButtonEventTrigger(GameObject obj, UnityAction callBack = null, string audioName = null, UnityAction<string> callBackAudio = null)
    {
        EventTrigger trigger;
        if (obj.GetComponent<EventTrigger>() != null)
        {
            trigger = obj.GetComponent<EventTrigger>();
        }
        else
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        if (callBack != null)
        {
            EventTrigger.Entry onclick = new EventTrigger.Entry();
            onclick.eventID = EventTriggerType.PointerUp;
            onclick.callback.AddListener((BaseEventData value) => { callBack(); });
            trigger.triggers.Add(onclick);
        }
        if (callBackAudio != null)
        {
            EventTrigger.Entry upclick = new EventTrigger.Entry();
            upclick.eventID = EventTriggerType.PointerDown;
            upclick.callback.AddListener((BaseEventData value) => { callBackAudio(audioName); });
            trigger.triggers.Add(upclick);
        }
    }


    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] button = this.GetComponentsInChildren<T>(true);
        for (int i = 0; i < button.Length; i++)
        {
            if (UIdict.ContainsKey(button[i].gameObject.name))
            {
                continue;
            }
            UIdict.Add(button[i].gameObject.name, button[i].gameObject);
        }
    }
}
