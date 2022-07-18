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

    private int nowWaveNum;//��ǰ����
    private WaveEnemy nowWaveEnemy;//��ǰ���ĵ���
    private int allEnemyNum = 0;//���е�����

    private void Awake()
    {
        GameMgr.Instance.RegisterEnemyPoint(this);
        nowWaveNum = enemyWaveList.Count;
        //���㵱ǰ���ֵ�����й��������ӵ�GameMgr��������
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
        //ȡ����������
        EnemyInfo info = DataMgr.Instance.enemyInfoList[nowWaveEnemy.enemyId];

        //��������Ԥ����
        ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
        {
            //��ʼ��
            obj.GetComponent<EnemyObj>().InitEnemy(info);

            obj.GetComponent<NavMeshAgent>().Warp(transform.position);
            obj.transform.rotation = transform.rotation;

            --nowWaveEnemy.enemyNum;

            //�����ǰ�����й�û����
            if (nowWaveEnemy.enemyNum > 0)
            {
                Invoke("CreateEnemy", perEnemyTime);
            }
            //�������ʣ�ನ��
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
    [Tooltip("һ�����˵�ID")]
    public int enemyId;
    [Tooltip("һ�����˵�����")]
    public int enemyNum;
}