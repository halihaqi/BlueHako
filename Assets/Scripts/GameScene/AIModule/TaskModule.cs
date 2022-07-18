using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskModule : AIModule
{
    protected override void Awake()
    {
        EnterAIMoudleEvent = (obj) =>
        {
            if (obj.GetComponent<TaskModule>() != null)
            {
                TaskMgr.EnterTask();
            }
        };
        base.Awake();
    }
}
