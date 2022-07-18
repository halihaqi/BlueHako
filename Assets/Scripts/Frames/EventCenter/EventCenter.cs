using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : Singleton<EventCenter>
{
    #region �ڲ���ͽӿ�
    /// <summary>
    /// ����սӿ�
    /// </summary>
    interface IEventInfo { }
    /// <summary>
    /// �в�ί��
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
    /// �޲�ί��
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



    //�¼�����,����ӿ�װ����
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ����¼�����(�в�)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��߼�</param>
    public void AddListener<T>(string name, UnityAction<T> action)
    {
        //����ֵ���������¼�����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //���û��
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }


    /// <summary>
    /// ����¼�����(�޲�)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��߼�</param>
    public void AddListener(string name, UnityAction action)
    {
        //����ֵ���������¼�����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        //���û��
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// �Ƴ��¼�����(�в�)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��߼�</param>
    public void RemoveListener<T>(string name, UnityAction<T> action)
    {
        //����ֵ���������¼�����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
        //���û��
        else
        {
            Debug.Log("Event don't has Listener " + name);
        }
    }


    /// <summary>
    /// �Ƴ��¼�����(�޲�)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��߼�</param>
    public void RemoveListener(string name, UnityAction action)
    {
        //����ֵ���������¼�����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
        //���û��
        else
        {
            Debug.Log("Event don't has Listener " + name);
        }
    }


    /// <summary>
    /// �㲥�¼�(�в�)
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="obj">�¼��Ĳ���</param>
    public void PostEvent<T>(string name, T obj)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions.Invoke(obj);
        //else
        //    Debug.Log("Post Event " + name + " has no Listener");
    }


    /// <summary>
    /// �㲥�¼�(�޲�)
    /// </summary>
    /// <param name="name">�¼���</param>
    public void PostEvent(string name)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions.Invoke();
        //else
        //    Debug.Log("Post Event " + name + " has no Listener");
    }


    /// <summary>
    /// �Ƴ��¼�
    /// </summary>
    /// <param name="name">�¼���</param>
    public void RemoveEvent(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic.Remove(name);
        }
        //���û��
        else
        {
            Debug.Log("No Remove Event " + name);
        }
    }


    /// <summary>
    /// ����¼�����
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

}
