using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{

    //按钮点击事件
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "BtnStart":
                //切换到选角界面，选完再切换到存档面板
                Begin_Cam.Instance.Move();
                UIMgr.Instance.HidePanel("BeginPanel");
                UIMgr.Instance.ShowPanel<ChooseRolePanel>("ChooseRolePanel", E_UI_Layer.Bot);
                break;
            case "BtnContinue":
                //无存档，该按钮已被隐藏
                //有存档，切换到选择存档面板
                UIMgr.Instance.ShowPanel<SaveLoadPanel>("SaveLoadPanel", E_UI_Layer.Bot, (panel) =>
                {
                    panel.slType = E_SaveOrLoad.Load;
                });
                break;
            case "BtnSettings":
                //切换到设置面板
                UIMgr.Instance.ShowPanel<SettingsPanel>("SettingsPanel", E_UI_Layer.Bot);
                break;
            case "BtnAbout":
                //切换到关于面板
                UIMgr.Instance.ShowPanel<AboutPanel>("AboutPanel", E_UI_Layer.Mid);
                break;
            case "BtnExit":
                //退出游戏
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public override void ShowMe()
    {
        //显示时判断是否有存档
        //无存档，隐藏继续按钮
        if (DataMgr.Instance.playerData.playerList.Count == 0)
            transform.DeepGetChild("BtnContinue").gameObject.SetActive(false);
        //调整游戏音乐为存档数据
        base.ShowMe();

        //添加进入游戏时的事件监听
        customEvent = () => { UIMgr.Instance.HidePanel("BeginPanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //移除进入游戏时的事件监听
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }
}
