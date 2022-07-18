using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    public Image imgHard;
    public Image imgNeed;
    public Image imgComplete;
    public Image imgReward;

    public Text txtName;
    public Text txtComplete;
    public Text txtReward;
    public Text txtTip;
    public Text txtBtnSure;

    private TaskInfo info;

    /// <summary>
    /// 改变面板显示的任务
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(TaskInfo info, bool isPlayer)
    {
        imgComplete.gameObject.SetActive(DataMgr.Instance.isTaskComplete(info));
        imgHard.sprite = ResMgr.Instance.Load<Sprite>("ItemImg/Img_Hard" + info.hard);
        imgNeed.sprite = ResMgr.Instance.Load<Sprite>(DataMgr.Instance.itemInfoList[info.needId].imgRes);
        imgReward.sprite = ResMgr.Instance.Load<Sprite>(DataMgr.Instance.itemInfoList[info.rewardId].imgRes);

        txtName.text = info.name;
        txtTip.text = info.tip;
        txtComplete.text = DataMgr.Instance.GetBagItemNum
            ((E_ItemType)DataMgr.Instance.itemInfoList[info.needId].id) + "/" + info.needNum;
        txtReward.text = "X" + info.rewardNum;
        this.info = info;

        if (isPlayer || DataMgr.Instance.isTaskComplete(info))
        {
            txtBtnSure.transform.parent.gameObject.SetActive(false);
            return;
        }
        txtBtnSure.text = DataMgr.Instance.isReciveTask(info) ? "提交" : "接取";
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if (btnName == "BtnClose")
            UIMgr.Instance.HidePanel("TaskPanel");
        else
        {
            if (txtBtnSure.text == "接取")
                TaskMgr.ReceiveTask(info);
            else
                TaskMgr.CompleteTask(info);
        }
    }
}
