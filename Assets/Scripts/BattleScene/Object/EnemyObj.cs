using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyObj : MonoBehaviour
{
    public Transform firePos;
    //�������������
    private EnemyInfo enemyInfo;
    //��ǰѪ��
    private int nowHp;
    //�Ƿ�����
    public bool isDead = false;
    //�ϴι���ʱ��
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

    #region �ⲿ����
    /// <summary>
    /// �ⲿ��ʼ������
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
    /// ����
    /// </summary>
    /// <param name="atk">�ܵ��˺�</param>
    public void Wound(int atk)
    {
        nowHp -= atk;
        if(nowHp <= 0)
        {
            nowHp = 0;
            Dead();
        }
        transform.Find("HpCanvas").GetComponent<HpObj>().UpdateHp((float)nowHp, (float)enemyInfo.hp);
        //����������Ч
        Debug.Log("Hit Enemy");
    }

    /// <summary>
    /// ����
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
        //����������Ч
        EventCenter.Instance.PostEvent<EnemyObj>("EnemyDead", this);
    }

    #endregion

    #region �����¼�
    //���������¼�
    public void BornOver()
    {
        agent.SetDestination(MainTowerObj.Instance.transform.position);
    }

    //�����¼�
    public void DeadEvent()
    {
        Destroy(gameObject);
        //�����Ϸʤ��
    }

    //�����¼�
    public void AtkEvent()
    {
        ResMgr.Instance.LoadAsync<GameObject>("EffRes/FireProjectileTiny", (obj) =>
        {
            obj.GetComponent<AutoFollow>().SetTarget(firePos, MainTowerObj.Instance.transform);
        });
        MainTowerObj.Instance.Wound(enemyInfo.atk);
        //������Ч
    }
    #endregion

    private void Update()
    {
        if (isDead)
            return;
        //�����ƶ���վ������
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
