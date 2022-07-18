using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTaskPanel : BasePanel
{
    public Transform notReceivedContent;
    public Transform receivedContent;
    public Transform completeContent;

    /// <summary>
    /// 更新任务列表
    /// </summary>
    /// <param name="state"></param>
    public void UpdateTaskList(int state, bool isPlayer)
    {
        ClearTaskList();
        foreach (TaskInfo item in DataMgr.Instance.taskInfoList)
        {
            if (item.hard <= state)
            {
                bool isRecive = false;
                //搜索人物是否接取任务,若没搜索到则isRecive为false
                foreach(string id in DataMgr.Instance.NowPlayerInfo.taskList.Keys)
                {
                    //如果人物已接取该任务,执行逻辑并跳出循环
                    if(item.id.ToString() == id)
                    {
                        isRecive = true;

                        //设置父对象
                        Transform parent = null;
                        //如果人物未完成该任务
                        if (DataMgr.Instance.NowPlayerInfo.taskList[id] == false)
                            parent = receivedContent;
                        //如果任务已完成
                        else
                            parent = completeContent;

                        LoadItem(item, parent, isPlayer);
                        break;
                    }
                }

                if (!isRecive)
                    LoadItem(item, notReceivedContent, isPlayer);
            }
        }
    }

    /// <summary>
    /// 清空任务列表
    /// </summary>
    private void ClearTaskList()
    {
        //此处不能用for或者foreach
        //因为Push的同时childCount也会改变
        while (notReceivedContent.childCount > 0)
        {
            PoolMgr.Instance.PushObj("UI/TaskListItem", notReceivedContent.GetChild(0).gameObject);
        }
        while(receivedContent.childCount > 0)
        {
            PoolMgr.Instance.PushObj("UI/TaskListItem", receivedContent.GetChild(0).gameObject);
        }
        while(completeContent.childCount > 0)
        {
            PoolMgr.Instance.PushObj("UI/TaskListItem", completeContent.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 加载任务列表元素
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    private void LoadItem(TaskInfo item, Transform parent, bool isPlayer)
    {
        PoolMgr.Instance.PopObj("UI/TaskListItem", (obj) =>
        {
            obj.transform.SetParent(parent, false);
            TaskListItem task = obj.GetComponent<TaskListItem>();
            task.taskName.text = DataMgr.Instance.taskInfoList[item.id].name;
            task.OnClick(() =>
            {
                UIMgr.Instance.ShowPanel<TaskPanel>("TaskPanel", E_UI_Layer.Mid, (panel) =>
                {
                    panel.ChangeInfo(item, isPlayer);
                });
            });
        });
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if (btnName == "BtnClose")
        {
            UIMgr.Instance.HidePanel("CheckTaskPanel");
            UIMgr.Instance.HidePanel("TaskPanel");
        }
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.PostEvent("ExitAIModule");
        EventCenter.Instance.PostEvent("PushTaskItem");
    }
}
