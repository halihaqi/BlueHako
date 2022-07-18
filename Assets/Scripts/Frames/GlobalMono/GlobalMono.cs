using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 全局Mono脚本，外部不调用，通过MonoMgr使用
/// </summary>
public class GlobalMono : MonoBehaviour
{
    private event UnityAction updateEvent;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        updateEvent?.Invoke();
    }

    /// <summary>
    /// 添加Update事件监听
    /// </summary>
    /// <param name="action">事件</param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 移除Update事件监听
    /// </summary>
    /// <param name="action">事件</param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

    /// <summary>
    /// 清空Update事件监听
    /// </summary>
    public void Clear()
    {
        updateEvent = null;
    }
}
