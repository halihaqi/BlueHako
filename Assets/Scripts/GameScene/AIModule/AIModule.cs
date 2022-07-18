using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AI模块基类，重写awake以添加监听对话结束的AI逻辑
/// </summary>
public abstract class AIModule : MonoBehaviour
{
    protected UnityAction<GameObject> EnterAIMoudleEvent;

    protected virtual void Awake()
    {
        EventCenter.Instance.AddListener<GameObject>("EnterAIModule", EnterAIMoudleEvent);
    }

    protected virtual void OnDestroy()
    {
        EventCenter.Instance.RemoveListener<GameObject>("EnterAIModule", EnterAIMoudleEvent);
    }
}
