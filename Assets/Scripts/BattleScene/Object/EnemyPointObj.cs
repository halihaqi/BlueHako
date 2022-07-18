using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPointObj : MonoBehaviour
{
    public List<WaveEnemy> enemyWaveList;
    public float firstWaveTime;
    public float perWaveTime;
    public float perEnemyTime;

    private int nowWaveNum;//当前波数
    private WaveEnemy nowWaveEnemy;//当前波的敌人
    private int allEnemyNum = 0;//所有敌人数

    private void Awake()
    {
        GameMgr.Instance.RegisterEnemyPoint(this);
        nowWaveNum = enemyWaveList.Count;
        //计算当前出怪点的所有怪物数并加到GameMgr怪物数中
        foreach (WaveEnemy item in enemyWaveList)
        {
            allEnemyNum += item.enemyNum;
        }
        Invoke("CreateWave", firstWaveTime);
        GameMgr.Instance.UpdateEnemyNum(allEnemyNum);
    }

    private void OnDestroy()
    {
        GameMgr.Instance.RemoveEnemyPoint(this);
    }

    private void CreateWave()
    {
        nowWaveEnemy = enemyWaveList[enemyWaveList.Count - nowWaveNum];
        CreateEnemy();
        --nowWaveNum;
    }

    private void CreateEnemy()
    {
        //取出怪物数据
        EnemyInfo info = DataMgr.Instance.enemyInfoList[nowWaveEnemy.enemyId];

        //创建怪物预设体
        ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
        {
            //初始化
            obj.GetComponent<EnemyObj>().InitEnemy(info);

            obj.GetComponent<NavMeshAgent>().Warp(transform.position);
            obj.transform.rotation = transform.rotation;

            --nowWaveEnemy.enemyNum;

            //如果当前波还有怪没生成
            if (nowWaveEnemy.enemyNum > 0)
            {
                Invoke("CreateEnemy", perEnemyTime);
            }
            //如果还有剩余波数
            else if (nowWaveNum > 0)
            {
                Invoke("CreateWave", perWaveTime);
            }
        });

    }

}

[System.Serializable]
public struct WaveEnemy
{
    [Tooltip("一波敌人的ID")]
    public int enemyId;
    [Tooltip("一波敌人的数量")]
    public int enemyNum;
}