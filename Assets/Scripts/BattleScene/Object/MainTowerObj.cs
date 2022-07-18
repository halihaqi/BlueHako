using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObj : SingletonMono<MainTowerObj>
{
    public SceneInfo nowInfo;
    private int nowMoney = 0;
    private int hp;
    private int maxHp;
    private int def = 0;//ͨ��BuffInfo����
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
                panel.ChangeInfo("��Q��Eѡ��BUFF����Tab����");
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

    #region �ⲿ����
    /// <summary>
    /// ��ʼ��������Ѫ��
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
    /// ���·��ص�Ѫ��
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
    /// ���½�Ǯ
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money)
    {
        nowMoney += money;
        UIMgr.Instance.GetPanel<BattlePanel>("BattlePanel").UpdateMoney(nowMoney);
    }

    /// <summary>
    /// ���ص�����
    /// </summary>
    /// <param name="atk">�ܵ��˺�</param>
    public void Wound(int atk)
    {
        if (isDead)
            return;
        hp -= ((atk - def) <= 0 ? 0 : (atk - def));
        //�����ûѪ��
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            //��ʾ�������
            GameMgr.Instance.LoseBattle();
        }
        UpdateHp(hp, maxHp);
    }
    
    #endregion
}
