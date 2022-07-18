using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    public TowerInfo info;
    public Transform firePos;
    private Animator anim;

    private float rotateSpeed = 10;

    private EnemyObj targetObj;
    private List<EnemyObj> targetObjs;

    private float frontAtkTime;
    private Vector3 targetPos;
    private int nowHp;
    private bool isDead = false;

    public void Init(TowerInfo info)
    {
        this.info = info;
        nowHp = info.hp;
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);
    }
    public void AtkEvent()
    {
        //生成特效

        //播放声音
        switch (info.type)
        {
            case 1://单体攻击
                if (targetObj == null)
                    return;
                //生成子弹特效
                ResMgr.Instance.LoadAsync<GameObject>(info.effRes, (obj) =>
                {
                    obj.GetComponent<AutoFollow>().SetTarget(firePos, targetObj.transform);
                });
                targetObj.Wound(info.atk);
                break;
            case 2://群体攻击
                //生成爆炸特效
                ResMgr.Instance.LoadAsync<GameObject>(info.effRes, (obj) =>
                {
                    obj.transform.position = targetPos;
                });
                foreach (EnemyObj enemy in targetObjs)
                {
                    if (!enemy.isDead)
                        enemy.Wound(info.atk);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="atk">受到伤害</param>
    public void Wound(int atk)
    {
        nowHp -= atk;
        if (nowHp <= 0)
        {
            nowHp = 0;
            Dead();
        }
        //播放受伤音效
        Debug.Log("Hit Enemy");
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        isDead = true;
    }

    private void Update()
    {
        if (isDead)
            return;
        //搜索敌人
        SearchTarget();
        //如果找不到，不执行下面逻辑
        if (targetObj == null)
            return;

        //如果旋转到夹角范围内
        //再进行攻击判断
        if (Mathf.Acos(Vector3.Dot(transform.forward.normalized, (targetPos - transform.position).normalized)) * Mathf.Rad2Deg < 5)
        {
            switch (info.type)
            {
                case 1://单体攻击
                    if (Time.time - frontAtkTime >= info.atkOffset)
                    {
                        frontAtkTime = Time.time;
                        //播放攻击动画
                        anim.SetTrigger("Atk");
                    }
                    break;
                case 2://群体攻击
                    //搜索范围攻击内所有敌人
                    targetObjs = GameMgr.Instance.FindEnemys(targetPos, info.aoeRange);
                    //如果找不到，不执行下面逻辑
                    if (targetObjs.Count == 0 || Time.time - frontAtkTime < info.atkOffset)
                        return;
                    frontAtkTime = Time.time;
                    //攻击敌人
                    anim.SetTrigger("Atk");
                    break;
                default:
                    break;
            }
        }
    }

    private void SearchTarget()
    {
        //搜索敌人
        if (targetObj == null || targetObj.isDead || Vector3.Distance
            (transform.position, targetObj.transform.position) > info.atkRange)
        {
            targetObj = GameMgr.Instance.FindOneEnemy(transform.position, info.atkRange);
        }
        //如果搜索不到,不执行下面逻辑
        if (targetObj == null)
        {
            anim.SetBool("Fire", false);
            return;
        }

        anim.SetBool("Fire", true);
        //旋转朝向敌人
        targetPos = targetObj.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
            (targetPos - transform.position), rotateSpeed * Time.deltaTime);
    }
}
