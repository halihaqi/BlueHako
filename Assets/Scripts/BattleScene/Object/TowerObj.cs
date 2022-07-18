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
        //������Ч

        //��������
        switch (info.type)
        {
            case 1://���幥��
                if (targetObj == null)
                    return;
                //�����ӵ���Ч
                ResMgr.Instance.LoadAsync<GameObject>(info.effRes, (obj) =>
                {
                    obj.GetComponent<AutoFollow>().SetTarget(firePos, targetObj.transform);
                });
                targetObj.Wound(info.atk);
                break;
            case 2://Ⱥ�幥��
                //���ɱ�ը��Ч
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
    /// ����
    /// </summary>
    /// <param name="atk">�ܵ��˺�</param>
    public void Wound(int atk)
    {
        nowHp -= atk;
        if (nowHp <= 0)
        {
            nowHp = 0;
            Dead();
        }
        //����������Ч
        Debug.Log("Hit Enemy");
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Dead()
    {
        isDead = true;
    }

    private void Update()
    {
        if (isDead)
            return;
        //��������
        SearchTarget();
        //����Ҳ�������ִ�������߼�
        if (targetObj == null)
            return;

        //�����ת���нǷ�Χ��
        //�ٽ��й����ж�
        if (Mathf.Acos(Vector3.Dot(transform.forward.normalized, (targetPos - transform.position).normalized)) * Mathf.Rad2Deg < 5)
        {
            switch (info.type)
            {
                case 1://���幥��
                    if (Time.time - frontAtkTime >= info.atkOffset)
                    {
                        frontAtkTime = Time.time;
                        //���Ź�������
                        anim.SetTrigger("Atk");
                    }
                    break;
                case 2://Ⱥ�幥��
                    //������Χ���������е���
                    targetObjs = GameMgr.Instance.FindEnemys(targetPos, info.aoeRange);
                    //����Ҳ�������ִ�������߼�
                    if (targetObjs.Count == 0 || Time.time - frontAtkTime < info.atkOffset)
                        return;
                    frontAtkTime = Time.time;
                    //��������
                    anim.SetTrigger("Atk");
                    break;
                default:
                    break;
            }
        }
    }

    private void SearchTarget()
    {
        //��������
        if (targetObj == null || targetObj.isDead || Vector3.Distance
            (transform.position, targetObj.transform.position) > info.atkRange)
        {
            targetObj = GameMgr.Instance.FindOneEnemy(transform.position, info.atkRange);
        }
        //�����������,��ִ�������߼�
        if (targetObj == null)
        {
            anim.SetBool("Fire", false);
            return;
        }

        anim.SetBool("Fire", true);
        //��ת�������
        targetPos = targetObj.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
            (targetPos - transform.position), rotateSpeed * Time.deltaTime);
    }
}
