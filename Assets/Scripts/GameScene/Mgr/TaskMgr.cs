using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMgr : Singleton<TaskMgr>
{
    public static bool isOpenTask = false;

    /// <summary>
    /// �򿪻�ر�Task���,��playerʹ��
    /// </summary>
    public static void OpenOrCloseTask()
    {
        if (UIMgr.Instance.GetPanel<CheckTaskPanel>("CheckTaskPanel") == null)
        {
            UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
            {
                //TODO:�޸�Ϊ�ý׶�index
                panel.UpdateTaskList(1, true);
            });
            isOpenTask = true;
        }
        else
        {
            isOpenTask = false;
            UIMgr.Instance.HidePanel("CheckTaskPanel");
            UIMgr.Instance.HidePanel("TaskPanel");
        }
    }

    /// <summary>
    /// ����������壬AIʹ��
    /// </summary>
    public static void EnterTask()
    {
        UIMgr.Instance.HideAllPanel("GamePanel");
        if (UIMgr.Instance.GetPanel<CheckTaskPanel>("CheckTaskPanel") == null)
        {
            UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
            {
                panel.UpdateTaskList(GameMapControllerObj.Instance.state, false);
            });
            isOpenTask = true;
        }
    }

    /// <summary>
    /// �뿪������壬AIʹ��
    /// </summary>
    public static void ExitTask()
    {
        isOpenTask = false;
        UIMgr.Instance.HidePanel("CheckTaskPanel");
        UIMgr.Instance.HidePanel("TaskPanel");
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="info"></param>
    public static void ReceiveTask(TaskInfo info)
    {
        //��ʾ�Ƿ�ȷ��
        UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
        {
            panel.ChangeTipInfo("�Ƿ��ȡ������", () =>
            {
                //�ȸı�����
                DataMgr.Instance.ReceiveTask(info.id);
                //�ٸı����
                if (isOpenTask)
                {
                    UIMgr.Instance.HidePanel("TaskPanel");
                    UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
                    {
                        panel.UpdateTaskList(GameMapControllerObj.Instance.state, false);
                    });
                }
            });
        });

    }

    /// <summary>
    /// �ύ����
    /// </summary>
    /// <param name="info"></param>
    public static void CompleteTask(TaskInfo info)
    {
        //��ʾ�Ƿ�ȷ��
        UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
        {
            //����ܹ����
            if (DataMgr.Instance.CompleteTask(info) == true)
            {
                //������ɶ�
                DataMgr.Instance.NowPlayerInfo.complete = DataMgr.Instance.NowPlayerInfo.complete + 5 <= 100 ?
                    DataMgr.Instance.NowPlayerInfo.complete + 5 : 100;
                //�ı����
                if (isOpenTask)
                {
                    UIMgr.Instance.HidePanel("TaskPanel");
                    UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
                    {
                        panel.UpdateTaskList(GameMapControllerObj.Instance.state, false);
                    });
                }
                panel.ChangeTipInfo("�������!", () =>
                {
                    panel.RewardTip((E_ItemType)info.rewardId, info.rewardNum);
                });
            }
            else
            {
                panel.ChangeTipInfo("���߲��㣬����δ���QAQ");
            }
        });
    }
}
