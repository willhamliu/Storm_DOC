using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 事件管理器
/// </summary>
public class EventManagement : InstanceNull<EventManagement>
{
    private Dictionary<string, UnityAction<GameObject>> eventDictionary = new Dictionary<string, UnityAction<GameObject>>();

    public void AddEvent(string eventname, UnityAction<GameObject> action)//添加事件
    {
        if (eventDictionary.ContainsKey(eventname))
        {
            eventDictionary[eventname] += action;
        }
        else
        {
            eventDictionary.Add(eventname,action);
        }
    }

    public void RemoveEvent(string eventname, UnityAction<GameObject> action)//删除事件
    {
        if (eventDictionary.ContainsKey(eventname))
        {
            eventDictionary[eventname] -= action;
        }
    }

    public void EventTrigger(string eventname,GameObject info)//事件触发
    {
        if (eventDictionary.ContainsKey(eventname))
        {
            eventDictionary[eventname].Invoke(info);
        }
    }

    public void CleanEvent()//清空事件
    {
        eventDictionary.Clear();
    }
}
