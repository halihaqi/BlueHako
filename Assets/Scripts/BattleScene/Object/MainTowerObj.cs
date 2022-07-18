using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObj : SingletonMono<MainTowerObj>
{
    public SceneInfo nowInfo;
    private int nowMoney = 0;
    private int hp;
    private int maxHp;
    private int def = 0;//通过BuffInfo增加
    private bool isDead;
    private float battleTime = 0;

    public int NowMoney => nowMoney;
    public int Hp 
    { 
        get { return hp; } 
        set
        {
            hp = value;
            if(hp > maxHp)
                hp = maxHp;
        }
    }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Def { get { return def; } set { def = value; } }
    public float BattleTime => battleTime;

    private void Update()
    {
        battleTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("按Q或E选择BUFF，按Tab提升");
            });
            UIMgr.Instance.ShowPanel<BuffPanel>("BuffPanel", E_UI_Layer.Bot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.HidePanel("GameTipPanel");
            UIMgr.Instance.HidePanel("BuffPanel");
        }
    }

    #region 外部调用
    /// <summary>
    /// 初始化防御点血量
    /// </summary>
    /// <param name="info"></param>
    public void InitMainTower(SceneInfo info)
    {
        maxHp = info.towerHp;
        hp = maxHp;
        nowMoney = info.money;

        nowInfo = info;
        UIMgr.Instance.ShowPanel<BattlePanel>("BattlePanel", E_UI_Layer.Bot, (panel) =>
        {
            panel.Init(info);
            panel.UpdateMoney(nowMoney);
            panel.UpdateHp(hp, maxHp);
            panel.UpdateEnemyNum(GameMgr.Instance.AllEnemyNum);
        });
    }

    /// <summary>
    /// 更新防守点血量
    /// </summary>
    /// <param name="nowHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHp(int nowHp, int maxHp)
    {
        this.hp = nowHp;
        this.maxHp = maxHp;
        UIMgr.Instance.GetPanel<BattlePanel>("BattlePanel").UpdateHp(nowHp, maxHp, def);
    }

    /// <summary>
    /// 更新金钱
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money)
    {
        nowMoney += money;
        UIMgr.Instance.GetPanel<BattlePanel>("BattlePanel").UpdateMoney(nowMoney);
    }

    /// <summary>
    /// 防守点受伤
    /// </summary>
    /// <param name="atk">受到伤害</param>
    public void Wound(int atk)
    {
        if (isDead)
            return;
        hp -= ((atk - def) <= 0 ? 0 : (atk - def));
        //如果塔没血了
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            //显示结束面板
            GameMgr.Instance.LoseBattle();
        }
        UpdateHp(hp, maxHp);
    }
    
    #endregion
}
