using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AIģ����࣬��дawake����Ӽ����Ի�������AI�߼�
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
