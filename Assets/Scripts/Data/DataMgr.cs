using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr>
{
    //��������
    public AudioData audioData;

    //�������
    public PlayerData playerData;

    //��������
    public BagData bagData;


    //��ɫ���ݱ�
    public List<RoleInfo> roleInfoList = new List<RoleInfo>();

    //ս���������ݱ�
    public List<SceneInfo> sceneInfoList = new List<SceneInfo>();

    //�������ݱ�
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

    //���������ݱ�
    public List<TowerInfo> towerInfoList = new List<TowerInfo>();

    //���������
    public List<ItemInfo> itemInfoList = new List<ItemInfo>();

    //�̵�����б�
    public List<ShopInfo> shopInfoList = new List<ShopInfo>();

    //�����б�
    public List<TaskInfo> taskInfoList = new List<TaskInfo>();

    //��ǰ�����Ϣ
    private PlayerInfo nowPlayerInfo;

    //��ǰ������Ϣ
    private SceneInfo nowSceneInfo;
    public SceneInfo NowSceneInfo { get { return nowSceneInfo; } set { nowSceneInfo = value; } }

    //���캯����ȡ������
    public DataMgr()
    {
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        enemyInfoList = JsonMgr.Instance.LoadData<List<EnemyInfo>>("EnemyInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
        itemInfoList = JsonMgr.Instance.LoadData<List<ItemInfo>>("ItemInfo");
        shopInfoList = JsonMgr.Instance.LoadData<List<ShopInfo>>("ShopInfo");
        taskInfoList = JsonMgr.Instance.LoadData<List<TaskInfo>>("TaskInfo");

        audioData = JsonMgr.Instance.LoadData<AudioData>("AudioData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        bagData = JsonMgr.Instance.LoadData<BagData>("BagData");
    }

    #region �������ݹ�������
    /// <summary>
    /// ������������
    /// </summary>
    public void SaveAudioData()
    {
        JsonMgr.Instance.SaveData(audioData, "AudioData");
    }
    #endregion

    #region ������ݹ�������
    /// <summary>
    /// ����������ݺͱ�������
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
        SaveBagData();
    }

    /// <summary>
    /// �����´浵���߸���ԭ���浵��������󱣴�
    /// ÿ�δ�����Ҵ浵����Ҫ����һ���µı����浵
    /// </summary>
    /// <param name="id">�浵id</param>
    /// <param name="roleId">��ѡ��ɫid</param>
    /// <returns>�´�����Info</returns>
    public PlayerInfo CreateNewPlayerInfo(int id, int roleId)
    {
        PlayerInfo newInfo = new PlayerInfo(id, roleId, 0, 0, 0);
        //������ڴ�id����Һͱ����浵�����Ǹô浵
        if (playerData.playerList.ContainsKey(id.ToString()))
        {
            playerData.playerList[id.ToString()] = newInfo;
            bagData.bagDic[id.ToString()] = new BagInfo();
        }
        else
        {
            playerData.playerList.Add(id.ToString(), newInfo);
            bagData.bagDic.Add(id.ToString(), new BagInfo());
        }
        nowPlayerInfo = newInfo;
        SavePlayerData();
        SaveBagData();
        return newInfo;
    }

    /// <summary>
    /// ��ȡ�浵
    /// </summary>
    /// <param name="id">�浵id</param>
    /// <returns></returns>
    public PlayerInfo LoadPlayerInfo(int id)
    {
        if (playerData.playerList.ContainsKey(id.ToString()))
            nowPlayerInfo = playerData.playerList[id.ToString()];
        return nowPlayerInfo;
    }

    /// <summary>
    /// ��ǰ�������
    /// </summary>
    public PlayerInfo NowPlayerInfo
    {
        get { return nowPlayerInfo; }
    }

    /// <summary>
    /// ͬ����ҽ�Ǯ�������н�Ǯ��
    /// </summary>
    public void UpdateMoney()
    {
        foreach(string id in NowBagInfo.slot.Keys)
        {
            //�������ʯ�͸���Ǯ
            if (NowBagInfo.slot[id].info.id == (int)E_ItemType.Diamond)
            {
                NowPlayerInfo.money = NowBagInfo.slot[id].num;
                return;
            }
        }
        //�������ʯ
        //���û�о�Ϊ0
        NowPlayerInfo.money = 0;

    }

    #endregion

    #region �������ݹ�������
    /// <summary>
    /// ���汳������
    /// </summary>
    public void SaveBagData()
    {
        JsonMgr.Instance.SaveData(bagData, "BagData");
    }

    /// <summary>
    /// ��ǰ��������
    /// </summary>
    public BagInfo NowBagInfo
    {
        get { return bagData.bagDic[nowPlayerInfo.id.ToString()]; }
    }

    /// <summary>
    /// ��ӵ���
    /// </summary>
    /// <param name="itemType">��������</param>
    /// <param name="num">��������</param>
    public bool AddItem(E_ItemType itemType, int num)
    {
        ItemInfo info = itemInfoList[(int)itemType];
        bool isFull = true;
        int emptyIndex = NowBagInfo.bagCapacity - 1;
        //���������Ϊ������ǰ��Ŀո�����ӵ���
        for (int i = NowBagInfo.bagCapacity - 1; i >= 0; i--)
        {
            //�����ǰ���߸�Ϊ��
            //��¼��ǰ���߸񣬲��Ҽ�¼����û����
            if (NowBagInfo.slot[i.ToString()].info == null)
            {
                emptyIndex = i;
                isFull = false;
            }
            //��������ֵ��ߣ����������󷵻�true
            if (NowBagInfo.slot[i.ToString()].info?.id == info.id)
            {
                NowBagInfo.slot[i.ToString()].num += num;
                //�������Ϊ9999
                if (NowBagInfo.slot[emptyIndex.ToString()].num > 9999)
                    NowBagInfo.slot[emptyIndex.ToString()].num = 9999;
                if (itemType == E_ItemType.Diamond)
                    NowPlayerInfo.money = NowBagInfo.slot[i.ToString()].num;
                return true;
            }
        }
        //��������û�иõ���
        //����û��������Ӹõ���
        if (!isFull)
        {
            NowBagInfo.slot[emptyIndex.ToString()].info = info;
            NowBagInfo.slot[emptyIndex.ToString()].num = num;
            if (NowBagInfo.slot[emptyIndex.ToString()].num > 9999)
                NowBagInfo.slot[emptyIndex.ToString()].num = 9999;
            return true;
        }
        //�������ˣ���ӡ����������ֱ�ӷ���
        else
        {
            Debug.Log("��������");
            return false;
        }
    }

    /// <summary>
    /// ȡ������
    /// </summary>
    /// <param name="itemType">��������</param>
    /// <param name="num">ȡ������</param>
    /// <returns>������������㹻ȡ������true�����㷵��false</returns>
    public bool RemoveItem(E_ItemType itemType, int num)
    {
        for (int i = 0; i < NowBagInfo.bagCapacity; i++)
        {
            //��������ֵ���
            if (NowBagInfo.slot[i.ToString()].info?.id == (int)itemType)
            {
                //������е�������Ҫ�Ƴ��ĵ�����
                //���Ƴ�������false
                if (NowBagInfo.slot[i.ToString()].num < num)
                {
                    Debug.Log("�õ��߳���������");
                    return false;
                }
                //���ӵ�п����Ƴ�����
                //���Ƴ�������true
                else
                {
                    NowBagInfo.slot[i.ToString()].num -= num;
                    //�������Ϊ0������ֵ����Ƴ�
                    if (NowBagInfo.slot[i.ToString()].num == 0)
                        NowBagInfo.slot[i.ToString()].info = null;
                    if (itemType == E_ItemType.Diamond)
                        NowPlayerInfo.money = NowBagInfo.slot[i.ToString()].num;
                    return true;
                }
            }
        }
        //���û�иõ��ߣ�����false
        Debug.Log("������û�иõ���");
        return false;
    }

    /// <summary>
    /// ��ȡ�����иõ��ߵ�����
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public int GetBagItemNum(E_ItemType itemType)
    {
        for (int i = 0; i < NowBagInfo.bagCapacity; i++)
        {
            ItemData data = NowBagInfo.slot[i.ToString()];
            if (data.info != null)
            {
                if (data.info.id == (int)itemType)
                {
                    return data.num;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// ��յ���
    /// </summary>
    public void ClearItem()
    {
        foreach(string id in NowBagInfo.slot.Keys)
        {
            NowBagInfo.slot[id].info = null;
            NowBagInfo.slot[id].num = 0;
        }
    }

    #endregion

    #region �̵����ݹ�������
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="id">���߸�id</param>
    /// <param name="num">������</param>
    public bool BuyItem(int id, int num)
    {
        E_ItemType moneyType = (E_ItemType)shopInfoList[id].moneyId;
        //����ܹ��Ƴ��Ž��������߼�
        if(RemoveItem(moneyType, shopInfoList[id].moneyNum * num))
        {
            E_ItemType itemType = (E_ItemType)shopInfoList[id].itemId;
            AddItem(itemType, num);
            return true;
        }
        else
        {
            Debug.Log("����ʧ��");
            return false;
        }
    }

    #endregion

    #region �������ݹ�������
    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="id">����id</param>
    public void ReceiveTask(int id)
    {
        if (!NowPlayerInfo.taskList.ContainsKey(id.ToString()))
            NowPlayerInfo.taskList.Add(id.ToString(), false);
        else
            Debug.Log("�ѽ�ȡ������");
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="id">����id</param>
    public bool CompleteTask(TaskInfo info)
    {
        if (NowPlayerInfo.taskList.ContainsKey(info.id.ToString()))
        {
            if (RemoveItem((E_ItemType)info.needId, info.needNum))
            {
                NowPlayerInfo.taskList[info.id.ToString()] = true;
                AddItem((E_ItemType)info.rewardId, info.rewardNum);
                return true;
            }
            else
                Debug.Log("���߲��㣬������ɸ�����");
        }
        else
            Debug.Log("δ��ȡ������");
        return false;
    }

    /// <summary>
    /// ���������Ƿ����
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool isTaskComplete(TaskInfo info)
    {
        bool isComplete = false;
        foreach(string id in NowPlayerInfo.taskList.Keys)
        {
            //����������id
            if (id == info.id.ToString())
            {
                isComplete = NowPlayerInfo.taskList[id];
                break;
            }
        }
        return isComplete;
    }

    /// <summary>
    /// ����Ƿ��ȡ������
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool isReciveTask(TaskInfo info)
    {
        bool isRecive = false;
        foreach (string id in NowPlayerInfo.taskList.Keys)
        {
            //����������id
            if (id == info.id.ToString())
            {
                isRecive = true;
                break;
            }
        }
        return isRecive;
    }
    #endregion

    #region ս�����ݹ�������
    /// <summary>
    /// ���ս�����¼ս�������
    /// </summary>
    /// <param name="id">ս������id</param>
    public void CompleteBattle(int id)
    {
        if (!NowPlayerInfo.battleCompleteList.Contains(id))
            NowPlayerInfo.battleCompleteList.Add(id);
        if(!NowPlayerInfo.battleCompleteList.Contains(id))
            NowPlayerInfo.complete = NowPlayerInfo.complete + 20 <= 100 ? NowPlayerInfo.complete + 20 : 100;
    }
    #endregion
}
