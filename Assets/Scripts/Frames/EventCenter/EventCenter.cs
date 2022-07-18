using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : Singleton<EventCenter>
{
    #region 内部类和接口
    /// <summary>
    /// 父类空接口
    /// </summary>
    interface IEventInfo { }
    /// <summary>
    /// 有参委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action)
        {
            actions = action;
        }
    }


    /// <summary>
    /// 无参委托
    /// </summary>
    class EventInfo : IEventInfo
    {
        public UnityAction actions;
        public EventInfo(UnityAction action)
        {
            actions = action;
        }
    }
    #endregion



    //事件容器,父类接口装子类
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听(有参)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">事件逻辑</param>
    public void AddListener<T>(string name, UnityAction<T> action)
    {
        //如果字典中有这个事件监听
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //如果没有
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }


    /// <summary>
    /// 添加事件监听(无参)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">事件逻辑</param>
    public void AddListener(string name, UnityAction action)
    {
        //如果字典中有这个事件监听
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        //如果没有
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// 移除事件监听(有参)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">事件逻辑</param>
    public void RemoveListener<T>(string name, UnityAction<T> action)
    {
        //如果字典中有这个事件监听
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
        //如果没有
        else
        {
            Debug.Log("Event don't has Listener " + name);
        }
    }


    /// <summary>
    /// 移除事件监听(无参)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">事件逻辑</param>
    public void RemoveListener(string name, UnityAction action)
    {
        //如果字典中有这个事件监听
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
        //如果没有
        else
        {
            Debug.Log("Event don't has Listener " + name);
        }
    }


    /// <summary>
    /// 广播事件(有参)
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="obj">事件的参数</param>
    public void PostEvent<T>(string name, T obj)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions.Invoke(obj);
        //else
        //    Debug.Log("Post Event " + name + " has no Listener");
    }


    /// <summary>
    /// 广播事件(无参)
    /// </summary>
    /// <param name="name">事件名</param>
    public void PostEvent(string name)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions.Invoke();
        //else
        //    Debug.Log("Post Event " + name + " has no Listener");
    }


    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="name">事件名</param>
    public void RemoveEvent(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic.Remove(name);
        }
        //如果没有
        else
        {
            Debug.Log("No Remove Event " + name);
        }
    }


    /// <summary>
    /// 清空事件容器
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

}
