using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    //玩家
    private PlayerObj player;
    public PlayerObj Player => player;

    //出怪点列表
    public List<EnemyPointObj> enemyPointList = new List<EnemyPointObj>();

    //怪物列表
    public List<EnemyObj> enemyList = new List<EnemyObj>();

    //怪物总数量
    private int allEnemyNum = 0;
    private int killEnemyNum = 0;
    public int AllEnemyNum => allEnemyNum;
    public int KillEnemyNum => killEnemyNum;

    #region Player公共方法
    /// <summary>
    /// 在场景中加载玩家并初始化
    /// </summary>
    public void InitPlayer(bool isBattle = false)
    {
        //加载当前玩家数据
        PlayerInfo playerInfo = DataMgr.Instance.NowPlayerInfo;
        RoleInfo roleInfo = DataMgr.Instance.roleInfoList[playerInfo.roleId];
        //创建玩家
        ResMgr.Instance.LoadAsync<GameObject>("Prefabs/Player/" + roleInfo.name, (obj) =>
        {
            //设置出生点
            Transform bornPos = GameObject.Find("BornPos").transform;
            obj.transform.position = bornPos.position;
            obj.transform.rotation = bornPos.rotation;

            //添加脚本和替换动画
            if (!isBattle)
                obj.AddComponent<PlayerObj>();
            else
                obj.AddComponent<BattlePlayerObj>();
            RuntimeAnimatorController controller = ResMgr.Instance.Load<RuntimeAnimatorController>
                ("Anim/GameScene/Player/" + roleInfo.name + "_Player");
            obj.GetComponent<Animator>().runtimeAnimatorController = controller;
            //注册到GameMgr中
            player = obj.GetComponent<PlayerObj>();
        });
    }
    #endregion

    #region 出怪点公共方法
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

    #region 敌人公共方法
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
    /// 找到距离最近的一个敌人
    /// </summary>
    /// <param name="ori">搜索起点</param>
    /// <param name="range">搜索范围</param>
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
    /// 找到所有在范围内的敌人
    /// </summary>
    /// <param name="ori">搜索起点</param>
    /// <param name="range">搜索范围</param>
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
    /// 检查游戏是否胜利
    /// 如果胜利弹出胜利面板
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
    /// 弹出失败面板
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
