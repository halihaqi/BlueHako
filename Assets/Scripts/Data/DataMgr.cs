using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : Singleton<DataMgr>
{
    //声音数据
    public AudioData audioData;

    //玩家数据
    public PlayerData playerData;

    //背包数据
    public BagData bagData;


    //角色数据表
    public List<RoleInfo> roleInfoList = new List<RoleInfo>();

    //战斗场景数据表
    public List<SceneInfo> sceneInfoList = new List<SceneInfo>();

    //敌人数据表
    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

    //防御塔数据表
    public List<TowerInfo> towerInfoList = new List<TowerInfo>();

    //道具种类表
    public List<ItemInfo> itemInfoList = new List<ItemInfo>();

    //商店道具列表
    public List<ShopInfo> shopInfoList = new List<ShopInfo>();

    //任务列表
    public List<TaskInfo> taskInfoList = new List<TaskInfo>();

    //当前玩家信息
    private PlayerInfo nowPlayerInfo;

    //当前场景信息
    private SceneInfo nowSceneInfo;
    public SceneInfo NowSceneInfo { get { return nowSceneInfo; } set { nowSceneInfo = value; } }

    //构造函数读取各数据
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

    #region 音乐数据公共方法
    /// <summary>
    /// 保存声音数据
    /// </summary>
    public void SaveAudioData()
    {
        JsonMgr.Instance.SaveData(audioData, "AudioData");
    }
    #endregion

    #region 玩家数据公共方法
    /// <summary>
    /// 保存玩家数据和背包数据
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
        SaveBagData();
    }

    /// <summary>
    /// 创建新存档或者覆盖原来存档，创建完后保存
    /// 每次创建玩家存档，都要创建一个新的背包存档
    /// </summary>
    /// <param name="id">存档id</param>
    /// <param name="roleId">所选角色id</param>
    /// <returns>新创建的Info</returns>
    public PlayerInfo CreateNewPlayerInfo(int id, int roleId)
    {
        PlayerInfo newInfo = new PlayerInfo(id, roleId, 0, 0, 0);
        //如果存在此id的玩家和背包存档，覆盖该存档
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
    /// 读取存档
    /// </summary>
    /// <param name="id">存档id</param>
    /// <returns></returns>
    public PlayerInfo LoadPlayerInfo(int id)
    {
        if (playerData.playerList.ContainsKey(id.ToString()))
            nowPlayerInfo = playerData.playerList[id.ToString()];
        return nowPlayerInfo;
    }

    /// <summary>
    /// 当前玩家数据
    /// </summary>
    public PlayerInfo NowPlayerInfo
    {
        get { return nowPlayerInfo; }
    }

    /// <summary>
    /// 同步玩家金钱到背包中金钱数
    /// </summary>
    public void UpdateMoney()
    {
        foreach(string id in NowBagInfo.slot.Keys)
        {
            //如果有钻石就更新钱
            if (NowBagInfo.slot[id].info.id == (int)E_ItemType.Diamond)
            {
                NowPlayerInfo.money = NowBagInfo.slot[id].num;
                return;
            }
        }
        //如果有钻石
        //如果没有就为0
        NowPlayerInfo.money = 0;

    }

    #endregion

    #region 背包数据公共方法
    /// <summary>
    /// 保存背包数据
    /// </summary>
    public void SaveBagData()
    {
        JsonMgr.Instance.SaveData(bagData, "BagData");
    }

    /// <summary>
    /// 当前背包数据
    /// </summary>
    public BagInfo NowBagInfo
    {
        get { return bagData.bagDic[nowPlayerInfo.id.ToString()]; }
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="itemType">道具种类</param>
    /// <param name="num">存入数量</param>
    public bool AddItem(E_ItemType itemType, int num)
    {
        ItemInfo info = itemInfoList[(int)itemType];
        bool isFull = true;
        int emptyIndex = NowBagInfo.bagCapacity - 1;
        //倒序遍历，为了在最前面的空格子添加道具
        for (int i = NowBagInfo.bagCapacity - 1; i >= 0; i--)
        {
            //如果当前道具格为空
            //记录当前道具格，并且记录背包没有满
            if (NowBagInfo.slot[i.ToString()].info == null)
            {
                emptyIndex = i;
                isFull = false;
            }
            //如果有这种道具，加上数量后返回true
            if (NowBagInfo.slot[i.ToString()].info?.id == info.id)
            {
                NowBagInfo.slot[i.ToString()].num += num;
                //道具最多为9999
                if (NowBagInfo.slot[emptyIndex.ToString()].num > 9999)
                    NowBagInfo.slot[emptyIndex.ToString()].num = 9999;
                if (itemType == E_ItemType.Diamond)
                    NowPlayerInfo.money = NowBagInfo.slot[i.ToString()].num;
                return true;
            }
        }
        //遍历后发现没有该道具
        //背包没有满，添加该道具
        if (!isFull)
        {
            NowBagInfo.slot[emptyIndex.ToString()].info = info;
            NowBagInfo.slot[emptyIndex.ToString()].num = num;
            if (NowBagInfo.slot[emptyIndex.ToString()].num > 9999)
                NowBagInfo.slot[emptyIndex.ToString()].num = 9999;
            return true;
        }
        //背包满了，打印背包已满，直接返回
        else
        {
            Debug.Log("背包已满");
            return false;
        }
    }

    /// <summary>
    /// 取出道具
    /// </summary>
    /// <param name="itemType">道具种类</param>
    /// <param name="num">取出数量</param>
    /// <returns>如果道具数量足够取出返回true，不足返回false</returns>
    public bool RemoveItem(E_ItemType itemType, int num)
    {
        for (int i = 0; i < NowBagInfo.bagCapacity; i++)
        {
            //如果有这种道具
            if (NowBagInfo.slot[i.ToString()].info?.id == (int)itemType)
            {
                //如果持有道具少于要移除的道具数
                //不移除并返回false
                if (NowBagInfo.slot[i.ToString()].num < num)
                {
                    Debug.Log("该道具持有数不足");
                    return false;
                }
                //如果拥有可以移除的量
                //则移除并返回true
                else
                {
                    NowBagInfo.slot[i.ToString()].num -= num;
                    //如果数量为0，则从字典中移除
                    if (NowBagInfo.slot[i.ToString()].num == 0)
                        NowBagInfo.slot[i.ToString()].info = null;
                    if (itemType == E_ItemType.Diamond)
                        NowPlayerInfo.money = NowBagInfo.slot[i.ToString()].num;
                    return true;
                }
            }
        }
        //如果没有该道具，返回false
        Debug.Log("背包中没有该道具");
        return false;
    }

    /// <summary>
    /// 获取背包中该道具的数量
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
    /// 清空道具
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

    #region 商店数据公共方法
    /// <summary>
    /// 购买道具
    /// </summary>
    /// <param name="id">道具格id</param>
    /// <param name="num">道具数</param>
    public bool BuyItem(int id, int num)
    {
        E_ItemType moneyType = (E_ItemType)shopInfoList[id].moneyId;
        //如果能够移除才进行以下逻辑
        if(RemoveItem(moneyType, shopInfoList[id].moneyNum * num))
        {
            E_ItemType itemType = (E_ItemType)shopInfoList[id].itemId;
            AddItem(itemType, num);
            return true;
        }
        else
        {
            Debug.Log("购买失败");
            return false;
        }
    }

    #endregion

    #region 任务数据公共方法
    /// <summary>
    /// 接取任务
    /// </summary>
    /// <param name="id">任务id</param>
    public void ReceiveTask(int id)
    {
        if (!NowPlayerInfo.taskList.ContainsKey(id.ToString()))
            NowPlayerInfo.taskList.Add(id.ToString(), false);
        else
            Debug.Log("已接取该任务");
    }

    /// <summary>
    /// 完成任务
    /// </summary>
    /// <param name="id">任务id</param>
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
                Debug.Log("道具不足，不能完成该任务");
        }
        else
            Debug.Log("未接取该任务");
        return false;
    }

    /// <summary>
    /// 检测该任务是否完成
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool isTaskComplete(TaskInfo info)
    {
        bool isComplete = false;
        foreach(string id in NowPlayerInfo.taskList.Keys)
        {
            //如果包含这个id
            if (id == info.id.ToString())
            {
                isComplete = NowPlayerInfo.taskList[id];
                break;
            }
        }
        return isComplete;
    }

    /// <summary>
    /// 检测是否接取该任务
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool isReciveTask(TaskInfo info)
    {
        bool isRecive = false;
        foreach (string id in NowPlayerInfo.taskList.Keys)
        {
            //如果包含这个id
            if (id == info.id.ToString())
            {
                isRecive = true;
                break;
            }
        }
        return isRecive;
    }
    #endregion

    #region 战斗数据公共方法
    /// <summary>
    /// 完成战斗后记录战斗已完成
    /// </summary>
    /// <param name="id">战斗场景id</param>
    public void CompleteBattle(int id)
    {
        if (!NowPlayerInfo.battleCompleteList.Contains(id))
        {
            NowPlayerInfo.complete = NowPlayerInfo.complete + 20 <= 100 ? NowPlayerInfo.complete + 20 : 100;
            NowPlayerInfo.battleCompleteList.Add(id);
        }
    }
    #endregion
}
