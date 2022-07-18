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
        //加载Canvas
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //加载EventSystem
        obj = ResMgr.Instance.Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 显示面板(异步)
    /// </summary>
    /// <typeparam name="T">面板类</typeparam>
    /// <param name="panelName">面板物体名</param>
    /// <param name="layer">面板要放的层级</param>
    /// <param name="callback">加载完的回调</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callback = null) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            callback?.Invoke(panelDic[panelName] as T);
            return;
        }

        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //判断父对象
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

            //设置父对象 初始化transform
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //获得物体挂载的panel脚本
            T panel = obj.GetComponent<T>();

            //添加到字典容器
            panelDic.Add(panelName, panel);
            panelDic[panelName].ShowMe();

            //执行回调
            callback?.Invoke(panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">面板名</param>
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
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">忽略的面板名</param>
    public void HideAllPanel(params string[] panelName)
    {
        List<string> keyList = new List<string>();
        //先将要忽略的key加入列表
        foreach (string ignore in panelName)
        {
            keyList.Add(ignore);
        }
        //再循环判断字典
        foreach(string key in panelDic.Keys)
        {
            //如果字典中有忽略的key，则移除该key
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
    /// 得到面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">面板名</param>
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
    /// 添加自定义事件监听
    /// </summary>
    /// <param name="control">控件</param>
    /// <param name="type">事件类型</param>
    /// <param name="callback">回调函数</param>
    public static void AddCustomListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        //添加EventTrigger组件
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        //添加自定义事件监听
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// 设置UI对象的UI层级
    /// </summary>
    /// <param name="rect">UI对象的RectTransform</param>
    /// <param name="layer">层级</param>
    /// <param name="worldPositionStays">如果为 true，则修改相对于父级的位置、缩放和旋转，使对象保持与之前相同的世界空间位置、旋转和缩放。</param>
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
