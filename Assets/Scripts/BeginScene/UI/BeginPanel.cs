using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{

    //��ť����¼�
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "BtnStart":
                //�л���ѡ�ǽ��棬ѡ�����л����浵���
                Begin_Cam.Instance.Move();
                UIMgr.Instance.HidePanel("BeginPanel");
                UIMgr.Instance.ShowPanel<ChooseRolePanel>("ChooseRolePanel", E_UI_Layer.Bot);
                break;
            case "BtnContinue":
                //�޴浵���ð�ť�ѱ�����
                //�д浵���л���ѡ��浵���
                UIMgr.Instance.ShowPanel<SaveLoadPanel>("SaveLoadPanel", E_UI_Layer.Bot, (panel) =>
                {
                    panel.slType = E_SaveOrLoad.Load;
                });
                break;
            case "BtnSettings":
                //�л����������
                UIMgr.Instance.ShowPanel<SettingsPanel>("SettingsPanel", E_UI_Layer.Bot);
                break;
            case "BtnAbout":
                //�л����������
                UIMgr.Instance.ShowPanel<AboutPanel>("AboutPanel", E_UI_Layer.Mid);
                break;
            case "BtnExit":
                //�˳���Ϸ
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public override void ShowMe()
    {
        //��ʾʱ�ж��Ƿ��д浵
        //�޴浵�����ؼ�����ť
        if (DataMgr.Instance.playerData.playerList.Count == 0)
            transform.DeepGetChild("BtnContinue").gameObject.SetActive(false);
        //������Ϸ����Ϊ�浵����
        base.ShowMe();

        //��ӽ�����Ϸʱ���¼�����
        customEvent = () => { UIMgr.Instance.HidePanel("BeginPanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //�Ƴ�������Ϸʱ���¼�����
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }
}
