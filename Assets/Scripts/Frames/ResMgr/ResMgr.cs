using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : Singleton<ResMgr>
{
    /// <summary>
    /// ������Դ(ͬ��)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">��Դ·��</param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //�����GameObject����ʵ�����ٷ���
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }

    /// <summary>
    /// ������Դ(�첽)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">��Դ·��</param>
    /// <param name="callback">�ص�����</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback = null) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(AsyncLoad(path, callback));
    }

    IEnumerator AsyncLoad<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest rr = Resources.LoadAsync<T>(path);
        yield return rr;
        if (rr.asset is GameObject)
            callback?.Invoke(GameObject.Instantiate(rr.asset) as T);
        else
            callback?.Invoke(rr.asset as T);
    }
}
