using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerData
{
    public Dictionary<string, PlayerInfo> playerList = new Dictionary<string, PlayerInfo>();
}

/// <summary>
/// 单个玩家数据
/// </summary>
public class PlayerInfo
{
    public int id;
    public int roleId;
    public int money;
    public int time;
    public float complete;
    public Dictionary<string, bool> taskList;
    public List<int> battleCompleteList;//记录完成的战斗的场景id
    public PlayerInfo() { }
    public PlayerInfo(int id, int roleId, int money, int time, float complete)
    {
        this.id = id;
        this.roleId = roleId;
        this.money = money;
        this.time = time;
        this.complete = complete;
        taskList = new Dictionary<string, bool>();
        battleCompleteList = new List<int>();
    }
}