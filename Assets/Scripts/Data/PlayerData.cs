using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// </summary>
public class PlayerData
{
    public Dictionary<string, PlayerInfo> playerList = new Dictionary<string, PlayerInfo>();
}

/// <summary>
/// �����������
/// </summary>
public class PlayerInfo
{
    public int id;
    public int roleId;
    public int money;
    public int time;
    public float complete;
    public Dictionary<string, bool> taskList;
    public List<int> battleCompleteList;//��¼��ɵ�ս���ĳ���id
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