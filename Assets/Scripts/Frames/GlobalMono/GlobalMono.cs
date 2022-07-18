using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ȫ��Mono�ű����ⲿ�����ã�ͨ��MonoMgrʹ��
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
    /// ���Update�¼�����
    /// </summary>
    /// <param name="action">�¼�</param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// �Ƴ�Update�¼�����
    /// </summary>
    /// <param name="action">�¼�</param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

    /// <summary>
    /// ���Update�¼�����
    /// </summary>
    public void Clear()
    {
        updateEvent = null;
    }
}
