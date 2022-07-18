using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskListItem : MonoBehaviour
{
    public Text taskName;
    public Button btnTask;
    private UnityAction pushEvent;

    public void OnClick(UnityAction action)
    {
        btnTask.onClick.RemoveAllListeners();
        btnTask.onClick.AddListener(action);
    }

    private void OnEnable()
    {
        pushEvent = () =>
        {
            PoolMgr.Instance.PushObj("UI/TaskListItem", gameObject);
        };
        EventCenter.Instance.AddListener("PushTaskItem", pushEvent);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener("PushTaskItem", pushEvent);
    }
}
