using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    //���
    private PlayerObj player;
    public PlayerObj Player => player;

    //���ֵ��б�
    public List<EnemyPointObj> enemyPointList = new List<EnemyPointObj>();

    //�����б�
    public List<EnemyObj> enemyList = new List<EnemyObj>();

    //����������
    private int allEnemyNum = 0;
    private int killEnemyNum = 0;
    public int AllEnemyNum => allEnemyNum;
    public int KillEnemyNum => killEnemyNum;

    #region Player��������
    /// <summary>
    /// �ڳ����м�����Ҳ���ʼ��
    /// </summary>
    public void InitPlayer(bool isBattle = false)
    {
        //���ص�ǰ�������
        PlayerInfo playerInfo = DataMgr.Instance.NowPlayerInfo;
        RoleInfo roleInfo = DataMgr.Instance.roleInfoList[playerInfo.roleId];
        //�������
        ResMgr.Instance.LoadAsync<GameObject>("Prefabs/Player/" + roleInfo.name, (obj) =>
        {
            //���ó�����
            Transform bornPos = GameObject.Find("BornPos").transform;
            obj.transform.position = bornPos.position;
            obj.transform.rotation = bornPos.rotation;

            //��ӽű����滻����
            if (!isBattle)
                obj.AddComponent<PlayerObj>();
            else
                obj.AddComponent<BattlePlayerObj>();
            RuntimeAnimatorController controller = ResMgr.Instance.Load<RuntimeAnimatorController>
                ("Anim/GameScene/Player/" + roleInfo.name + "_Player");
            obj.GetComponent<Animator>().runtimeAnimatorController = controller;
            //ע�ᵽGameMgr��
            player = obj.GetComponent<PlayerObj>();
        });
    }
    #endregion

    #region ���ֵ㹫������
    public void RegisterEnemyPoint(EnemyPointObj point)
    {
        enemyPointList.Add(point);
    }

    public void RemoveEnemyPoint(EnemyPointObj point)
    {
        enemyPointList.Remove(point);
    }

    public void ClearEnemyPoint()
    {
        enemyPointList.Clear();
    }
    #endregion

    #region ���˹�������
    public void RegisterEnemy(EnemyObj enemy)
    {
        enemyList.Add(enemy);
    }

    public void RemoveEnemy(EnemyObj enemy)
    {
        enemyList.Remove(enemy);
    }

    public void ClearEnemy()
    {
        enemyList.Clear();
    }

    public void UpdateEnemyNum(int num)
    {
        allEnemyNum += num;
    }

    public void UpdateKillEnemyNum(int num)
    {
        killEnemyNum += num;
    }

    /// <summary>
    /// �ҵ����������һ������
    /// </summary>
    /// <param name="ori">�������</param>
    /// <param name="range">������Χ</param>
    /// <returns></returns>
    public EnemyObj FindOneEnemy(Vector3 ori, float range)
    {
        EnemyObj target = null;
        float minDis = 100;
        foreach(EnemyObj enemy in enemyList)
        {
            if (enemy == null)
                continue;
            float dis = Vector3.Distance(ori, enemy.transform.position);
            if(dis < range && dis < minDis)
            {
                target = enemy;
                minDis = dis;
            }
        }
        return target;
    }

    /// <summary>
    /// �ҵ������ڷ�Χ�ڵĵ���
    /// </summary>
    /// <param name="ori">�������</param>
    /// <param name="range">������Χ</param>
    /// <returns></returns>
    public List<EnemyObj> FindEnemys(Vector3 ori, float range)
    {
        List<EnemyObj> findList = new List<EnemyObj>();
        foreach (EnemyObj enemy in enemyList)
        {
            if (enemy == null)
                continue;
            float dis = Vector3.Distance(ori, enemy.transform.position);
            if (dis < range)
                findList.Add(enemy);
        }
        return findList;
    }

    /// <summary>
    /// �����Ϸ�Ƿ�ʤ��
    /// ���ʤ������ʤ�����
    /// </summary>
    /// <returns></returns>
    public bool CheckVictory()
    {
        if (allEnemyNum > 0)
            return false;
        else
        {
            UIMgr.Instance.ShowPanel<WinPanel>("WinPanel", E_UI_Layer.System, (panel) =>
            {
                panel.Init(true);
            });
            return true;
        }
    }

    /// <summary>
    /// ����ʧ�����
    /// </summary>
    public void LoseBattle()
    {
        UIMgr.Instance.ShowPanel<WinPanel>("WinPanel", E_UI_Layer.System, (panel) =>
        {
            panel.Init(false);
        });
    }
    #endregion


}
