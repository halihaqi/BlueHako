using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    public Image imgRank;
    public Text txtWinLose;
    public Text txtHpPer;
    public Text txtEnemyHit;
    public Text txtTime;
    public Text txtReward;

    public void Init(bool isWin)
    {
        txtWinLose.text = isWin ? "Win" : "Lose";
        float hpPer = (float)MainTowerObj.Instance.Hp / (float)MainTowerObj.Instance.MaxHp * 100;
        txtHpPer.text = "物资完整度:"+ (int)hpPer + "%";
        txtEnemyHit.text = "击败敌人：" + GameMgr.Instance.KillEnemyNum;
        txtTime.text = "时间:" + ((int)MainTowerObj.Instance.BattleTime).ToTime();
        txtReward.text = "X" + (isWin ? MainTowerObj.Instance.nowInfo.reward : 0);
        imgRank.sprite = ResMgr.Instance.Load<Sprite>("RankImg/Rank" + (int)hpPer / 25);
    }


    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        DataMgr.Instance.AddItem(E_ItemType.Diamond, (int)(MainTowerObj.Instance.nowInfo.reward * (float)MainTowerObj.Instance.Hp / MainTowerObj.Instance.MaxHp));
        if (txtWinLose.text == "Win")
            DataMgr.Instance.CompleteBattle(MainTowerObj.Instance.nowInfo.id);

        UIMgr.Instance.HideAllPanel();
        SceneMgr.Instance.LoadSceneAsyncPro("GameScene", () =>
        {
            GameMgr.Instance.InitPlayer();
            UIMgr.Instance.ShowPanel<GamePanel>("GamePanel", E_UI_Layer.Bot, (panel) =>
            {
                panel.Init(DataMgr.Instance.NowPlayerInfo);
            });
        });
    }
}
