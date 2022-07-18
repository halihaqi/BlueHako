using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包数据类，记录所有人物背包
/// </summary>
public class BagData
{
    /// <summary>
    /// string与人物id相同，BagInfo为当前人物的背包数据类
    /// </summary>
    public Dictionary<string, BagInfo> bagDic = new Dictionary<string, BagInfo>();
}

/// <summary>
/// 背包单个数据结构
/// </summary>
public class ItemData
{
    public ItemInfo info = null;
    public int num = 0;
}

/// <summary>
/// 当前人物的背包数据类
/// </summary>
public class BagInfo
{
    /// <summary>
    /// string为背包格id,ItemData为每格对应的道具信息和道具数
    /// </summary>
    public Dictionary<string, ItemData> slot;
    /// <summary>
    /// 背包容量
    /// </summary>
    public int bagCapacity = 16;

    /// <summary>
    /// 根据背包最大容量初始化空背包
    /// </summary>
    public BagInfo()
    {
        slot = new Dictionary<string, ItemData>(bagCapacity);
        for (int i = 0; i < bagCapacity; i++)
        {
            slot.Add(i.ToString(), new ItemData());
        }
    }
}
