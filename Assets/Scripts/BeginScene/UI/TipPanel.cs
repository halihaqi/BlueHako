using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public GameObject rewardPanel;
    public Text txtTip;
    public Text txtReward;
    public Image imgReward;

    private UnityAction sureAction;
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if(btnName == "BtnSure")
        {
            sureAction?.Invoke();
        }
        UIMgr.Instance.HidePanel("TipPanel");
    }

    /// <summary>
    /// 改变提示和添加确定后事件
    /// </summary>
    /// <param name="info">提示文字</param>
    /// <param name="sureAction">按下确定后的事件</param>
    public void ChangeTipInfo(string info, UnityAction sureAction = null)
    {
        txtTip.gameObject.SetActive(true);
        rewardPanel.SetActive(false);
        txtTip.text = info;
        this.sureAction = sureAction;
    }

    /// <summary>
    /// 显示奖励面板信息，隐藏提示面板
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="num"></param>
    public void RewardTip(E_ItemType itemType, int num, UnityAction sureAction = null)
    {
        txtTip.gameObject.SetActive(false);
        rewardPanel.SetActive(true);
        txtReward.text = "X" + num;
        imgReward.sprite = ResMgr.Instance.Load<Sprite>
            (DataMgr.Instance.itemInfoList[(int)itemType].imgRes);
        this.sureAction = sureAction;
    }
}
