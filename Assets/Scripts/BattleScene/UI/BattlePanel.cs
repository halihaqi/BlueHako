using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    public Image imgTower;
    public Text txtMoney;
    public Text txtEnemy;
    public Text txtTime;
    public Text txtHp;
    public Text txtDef;

    public Slider sliderHp;

    private void Update()
    {
        if (MainTowerObj.Instance != null)
            txtTime.text = ((int)MainTowerObj.Instance.BattleTime).ToTime();
    }

    #region 外部调用
    /// <summary>
    /// 更新金钱数
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = "X " + money;
    }

    public void Init(SceneInfo info)
    {
        imgTower.sprite = ResMgr.Instance.Load<Sprite>(info.towerImgRes);
    }

    /// <summary>
    /// 更新面板中的防守点血量
    /// </summary>
    /// <param name="nowHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHp(float nowHp, float maxHp, int def = 0)
    {
        sliderHp.value = nowHp / maxHp;
        txtHp.text = nowHp + "/" + maxHp;
        txtDef.text = def.ToString();
    }

    /// <summary>
    /// 更新敌人数量
    /// </summary>
    /// <param name="num"></param>
    public void UpdateEnemyNum(int num)
    {
        txtEnemy.text = num.ToString();
    }
    #endregion

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if (btnName == "BtnSetting")
            UIMgr.Instance.ShowPanel<ExitPanel>("ExitPanel", E_UI_Layer.System, (panel) =>
            {
                panel.BattleSceneMode();
            });
    }
}
