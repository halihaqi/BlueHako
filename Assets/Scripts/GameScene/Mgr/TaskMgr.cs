using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMgr : Singleton<TaskMgr>
{
    public static bool isOpenTask = false;

    /// <summary>
    /// 打开或关闭Task面板,供player使用
    /// </summary>
    public static void OpenOrCloseTask()
    {
        if (UIMgr.Instance.GetPanel<CheckTaskPanel>("CheckTaskPanel") == null)
        {
            UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
            {
                //TODO:修改为该阶段index
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
    /// 进入任务面板，AI使用
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
    /// 离开任务面板，AI使用
    /// </summary>
    public static void ExitTask()
    {
        isOpenTask = false;
        UIMgr.Instance.HidePanel("CheckTaskPanel");
        UIMgr.Instance.HidePanel("TaskPanel");
    }

    /// <summary>
    /// 接受任务
    /// </summary>
    /// <param name="info"></param>
    public static void ReceiveTask(TaskInfo info)
    {
        //提示是否确定
        UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
        {
            panel.ChangeTipInfo("是否接取该任务？", () =>
            {
                //先改变数据
                DataMgr.Instance.ReceiveTask(info.id);
                //再改变面板
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
    /// 提交任务
    /// </summary>
    /// <param name="info"></param>
    public static void CompleteTask(TaskInfo info)
    {
        //提示是否确定
        UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
        {
            //如果能够完成
            if (DataMgr.Instance.CompleteTask(info) == true)
            {
                //增加完成度
                DataMgr.Instance.NowPlayerInfo.complete = DataMgr.Instance.NowPlayerInfo.complete + 5 <= 100 ?
                    DataMgr.Instance.NowPlayerInfo.complete + 5 : 100;
                //改变面板
                if (isOpenTask)
                {
                    UIMgr.Instance.HidePanel("TaskPanel");
                    UIMgr.Instance.ShowPanel<CheckTaskPanel>("CheckTaskPanel", E_UI_Layer.Bot, (panel) =>
                    {
                        panel.UpdateTaskList(GameMapControllerObj.Instance.state, false);
                    });
                }
                panel.ChangeTipInfo("任务完成!", () =>
                {
                    panel.RewardTip((E_ItemType)info.rewardId, info.rewardNum);
                });
            }
            else
            {
                panel.ChangeTipInfo("道具不足，任务未完成QAQ");
            }
        });
    }
}
