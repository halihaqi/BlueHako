using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTaskPanel : BasePanel
{
    public Transform notReceivedContent;
    public Transform receivedContent;
    public Transform completeContent;

    /// <summary>
    /// ���������б�
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
                //���������Ƿ��ȡ����,��û��������isReciveΪfalse
                foreach(string id in DataMgr.Instance.NowPlayerInfo.taskList.Keys)
                {
                    //��������ѽ�ȡ������,ִ���߼�������ѭ��
                    if(item.id.ToString() == id)
                    {
                        isRecive = true;

                        //���ø�����
                        Transform parent = null;
                        //�������δ��ɸ�����
                        if (DataMgr.Instance.NowPlayerInfo.taskList[id] == false)
                            parent = receivedContent;
                        //������������
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
    /// ��������б�
    /// </summary>
    private void ClearTaskList()
    {
        //�˴�������for����foreach
        //��ΪPush��ͬʱchildCountҲ��ı�
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
    /// ���������б�Ԫ��
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
