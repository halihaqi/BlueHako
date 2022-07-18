using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    /// <summary>
    /// 加载场景(同步)
    /// </summary>
    /// <param name="name">场景名</param>
    public void LoadScene(string name)
    {
        PoolMgr.Instance.Clear();
        SceneManager.LoadScene(name);
        EventCenter.Instance.Clear();
    }

    /// <summary>
    /// 加载场景(异步)
    /// 加载过程中会分发事件"Loading"
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">完成回调函数</param>
    public void LoadSceneAsync(string name, UnityAction callback)
    {
        //加载场景前要清空对象池字典，否则会出错
        PoolMgr.Instance.Clear();
        MonoMgr.Instance.StartCoroutine(AsyncLoad(name, callback));
    }

    /// <summary>
    /// 加载场景(异步Pro)
    /// 先显示加载面板，再加载
    /// 加载完成后等待waitTime才隐藏加载面板
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="callback">加载完成的回调</param>
    /// <param name="waitTime">加载完成后加载面板延迟消失的时间</param>
    public void LoadSceneAsyncPro(string name, UnityAction callback, float waitTime = 0.5f)
    {
        //加载场景前要清空对象池字典，否则会出错
        PoolMgr.Instance.Clear();
        //先显示加载面板，再开启协程加载
        UIMgr.Instance.ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.System, (panel) =>
        {
            MonoMgr.Instance.StartCoroutine(AsyncLoadPro(name, callback, waitTime));
        });
    }


    IEnumerator AsyncLoad(string name,UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return ao.progress;
        }
        EventCenter.Instance.PostEvent("LoadComplete");
        action?.Invoke();
    }

    IEnumerator AsyncLoadPro(string name, UnityAction callback, float waitTime)
    {
        int toProgress = 0;
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //加载到0.9就加载完了
        ao.allowSceneActivation = false;
        yield return new WaitForEndOfFrame();
        while (ao.progress < 0.9f)
        {
            toProgress = (int)(ao.progress * 100);
            EventCenter.Instance.PostEvent("Loading", toProgress);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        ao.allowSceneActivation = true;
        while (!ao.isDone)
            yield return new WaitForEndOfFrame();
        callback?.Invoke();
        //这里是为了延迟隐藏Loading面板
        yield return new WaitForSeconds(waitTime);
        while (toProgress < 100)
        {
            toProgress++;
            EventCenter.Instance.PostEvent("Loading", toProgress);
            yield return new WaitForEndOfFrame();
        }
        EventCenter.Instance.PostEvent("LoadComplete");
    }
}
