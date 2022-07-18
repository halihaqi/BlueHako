using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Text txtMoney;
    public Text txtName;

    public Image imgIcon;

    /// <summary>
    /// 初始化游戏面板
    /// </summary>
    public void Init(PlayerInfo nowPlayerInfo)
    {
        RoleInfo roleInfo = DataMgr.Instance.roleInfoList[nowPlayerInfo.roleId];
        txtName.text = roleInfo.name;
        txtMoney.text = "X " + nowPlayerInfo.money.ToString();
        imgIcon.sprite = ResMgr.Instance.Load<Sprite>(roleInfo.imgRes);
    }

    public void UpdateMoney()
    {
        txtMoney.text = "X " + DataMgr.Instance.NowPlayerInfo.money.ToString();
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.ShowPanel<ExitPanel>("ExitPanel", E_UI_Layer.System);
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Init(DataMgr.Instance.NowPlayerInfo);
    }
}
