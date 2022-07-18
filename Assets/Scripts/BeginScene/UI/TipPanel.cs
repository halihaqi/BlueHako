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
    /// �ı���ʾ�����ȷ�����¼�
    /// </summary>
    /// <param name="info">��ʾ����</param>
    /// <param name="sureAction">����ȷ������¼�</param>
    public void ChangeTipInfo(string info, UnityAction sureAction = null)
    {
        txtTip.gameObject.SetActive(true);
        rewardPanel.SetActive(false);
        txtTip.text = info;
        this.sureAction = sureAction;
    }

    /// <summary>
    /// ��ʾ���������Ϣ��������ʾ���
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
