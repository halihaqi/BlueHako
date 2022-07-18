using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyObj : MonoBehaviour
{
    public Transform firePos;
    //敌人自身的数据
    private EnemyInfo enemyInfo;
    //当前血量
    private int nowHp;
    //是否死亡
    public bool isDead = false;
    //上次攻击时间
    private float frontAtkTime;

    //Component
    private Animator anim;
    private NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GameMgr.Instance.RegisterEnemy(this);
    }

    #region 外部调用
    /// <summary>
    /// 外部初始化敌人
    /// </summary>
    /// <param name="info"></param>
    public void InitEnemy(EnemyInfo info)
    {
        enemyInfo = info;
        agent.speed = info.moveSpeed;
        agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.rotateSpeed;
        nowHp = info.hp;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="atk">受到伤害</param>
    public void Wound(int atk)
    {
        nowHp -= atk;
        if(nowHp <= 0)
        {
            nowHp = 0;
            Dead();
        }
        transform.Find("HpCanvas").GetComponent<HpObj>().UpdateHp((float)nowHp, (float)enemyInfo.hp);
        //播放受伤音效
        Debug.Log("Hit Enemy");
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        isDead = true;
        agent.isStopped = true;
        anim.SetBool("Dead", true);
        GameMgr.Instance.RemoveEnemy(this);
        GameMgr.Instance.UpdateEnemyNum(-1);
        UIMgr.Instance.GetPanel<BattlePanel>("BattlePanel").UpdateEnemyNum(GameMgr.Instance.AllEnemyNum);
        GameMgr.Instance.UpdateKillEnemyNum(1);
        MainTowerObj.Instance.UpdateMoney(enemyInfo.money);
        BagMgr.Instance.AddItem((E_ItemType)enemyInfo.rewardId, 1);
        GameMgr.Instance.CheckVictory();
        //播放死亡音效
        EventCenter.Instance.PostEvent<EnemyObj>("EnemyDead", this);
    }

    #endregion

    #region 动画事件
    //出生结束事件
    public void BornOver()
    {
        agent.SetDestination(MainTowerObj.Instance.transform.position);
    }

    //死亡事件
    public void DeadEvent()
    {
        Destroy(gameObject);
        //检测游戏胜利
    }

    //攻击事件
    public void AtkEvent()
    {
        ResMgr.Instance.LoadAsync<GameObject>("EffRes/FireProjectileTiny", (obj) =>
        {
            obj.GetComponent<AutoFollow>().SetTarget(firePos, MainTowerObj.Instance.transform);
        });
        MainTowerObj.Instance.Wound(enemyInfo.atk);
        //播放音效
    }
    #endregion

    private void Update()
    {
        if (isDead)
            return;
        //设置移动或站立动画
        anim.SetFloat("Speed", agent.velocity.magnitude);

        //ATK
        if(Vector3.Distance(transform.position, MainTowerObj.Instance.transform.position) <= enemyInfo.atkRange)
        {
            anim.SetBool("Fire", true);
            agent.isStopped = true;
            if(Time.time - frontAtkTime >= enemyInfo.atkOffset)
            {
                frontAtkTime = Time.time;
                transform.LookAt(MainTowerObj.Instance.transform.position);
                anim.SetTrigger("Atk");
            }
        }
        else
        {
            anim.SetBool("Fire", false);
            agent.isStopped = false;
        }

    }

}
