using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bot,Mid,Top,System
}
public class UIMgr : Singleton<UIMgr>
{
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    public RectTransform canvas;

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public UIMgr()
    {
        //����Canvas
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //����EventSystem
        obj = ResMgr.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// ��ʾ���(�첽)
    /// </summary>
    /// <typeparam name="T">�����</typeparam>
    /// <param name="panelName">���������</param>
    /// <param name="layer">���Ҫ�ŵĲ㼶</param>
    /// <param name="callback">������Ļص�</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callback = null) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            callback?.Invoke(panelDic[panelName] as T);
            return;
        }

        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //�жϸ�����
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }

            //���ø����� ��ʼ��transform
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //���������ص�panel�ű�
            T panel = obj.GetComponent<T>();

            //��ӵ��ֵ�����
            panelDic.Add(panelName, panel);
            panelDic[panelName].ShowMe();

            //ִ�лص�
            callback?.Invoke(panel);
        });
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject, 1);
            panelDic.Remove(panelName);
        }
        else
        {
            Debug.Log("NO Hide Panel " + panelName);
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">���Ե������</param>
    public void HideAllPanel(params string[] panelName)
    {
        List<string> keyList = new List<string>();
        //�Ƚ�Ҫ���Ե�key�����б�
        foreach (string ignore in panelName)
        {
            keyList.Add(ignore);
        }
        //��ѭ���ж��ֵ�
        foreach(string key in panelDic.Keys)
        {
            //����ֵ����к��Ե�key�����Ƴ���key
            if (keyList.Contains(key))
                keyList.Remove(key);
            else
                keyList.Add(key);
        }
        foreach (string key in keyList)
        {
            HidePanel(key);
        }
    }

    /// <summary>
    /// �õ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">�����</param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        else
            Debug.Log("NO Get Panel " + panelName);
        return null;
    }

    /// <summary>
    /// ����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callback">�ص�����</param>
    public static void AddCustomListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        //���EventTrigger���
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        //����Զ����¼�����
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// ����UI�����UI�㼶
    /// </summary>
    /// <param name="rect">UI�����RectTransform</param>
    /// <param name="layer">�㼶</param>
    /// <param name="worldPositionStays">���Ϊ true�����޸�����ڸ�����λ�á����ź���ת��ʹ���󱣳���֮ǰ��ͬ������ռ�λ�á���ת�����š�</param>
    public void SetUILayer(RectTransform rect, E_UI_Layer layer, bool worldPositionStays)
    {
        Transform parent = bot;
        switch (layer)
        {
            case E_UI_Layer.Bot:
                parent = bot;
                break;
            case E_UI_Layer.Mid:
                parent = mid;
                break;
            case E_UI_Layer.Top:
                parent = top;
                break;
            case E_UI_Layer.System:
                parent = system;
                break;
        }
        rect.SetParent(parent, worldPositionStays);
    }
}
