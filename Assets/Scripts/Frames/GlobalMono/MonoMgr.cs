using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : Singleton<MonoMgr>
{
    private GlobalMono globalMono;

    public MonoMgr()
    {
        GameObject obj = new GameObject("GlobalMono");
        //GlobalMono的Start()中有过场景不销毁
        globalMono = obj.AddComponent<GlobalMono>();
    }

    #region Update相关公共方法
    /// <summary>
    /// 添加Update事件监听
    /// </summary>
    /// <param name="action">事件</param>
    public void AddUpdateListener(UnityAction action)
    {
        globalMono.AddUpdateListener(action);
    }

    /// <summary>
    /// 移除Update事件监听
    /// </summary>
    /// <param name="action">事件</param>
    public void RemoveUpdateListener(UnityAction action)
    {
        globalMono.RemoveUpdateListener(action);
    }

    /// <summary>
    /// 清空Update事件监听
    /// </summary>
    public void Clear()
    {
        globalMono.Clear();
    }
    #endregion

    #region Coroutine相关公共方法(只有基本的，可拓展)
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return globalMono.StartCoroutine(routine);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        globalMono.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        globalMono.StopCoroutine(routine);
    }

    public void StopAllCoroutines()
    {
        globalMono.StopAllCoroutines();
    }
    #endregion


}
