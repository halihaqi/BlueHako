using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具种类,
/// 和DataMgr.Instance.itemInfoList[i]一一对应
/// </summary>
public enum E_ItemType
{
    Diamond, Gold,
    TowerUp1, TowerUp2, TowerUp3, 
    MainUp1, MainUp2,
    QuestBox,
    Cat, Enemy1, Enemy2, Enemy3
}

public class BagMgr : Singleton<BagMgr>
{
    public bool isOpenBag = false;

    #region 背包公共方法
    /// <summary>
    /// 打开或关闭背包
    /// </summary>
    public void OpenOrCloseBag()
    {
        if (UIMgr.Instance.GetPanel<BagPanel>("BagPanel") == null)
        {
            UIMgr.Instance.ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Bot);
            isOpenBag = true;
        }
        else
        {
            isOpenBag = false;
            UIMgr.Instance.HidePanel("BagPanel");
        }
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="num"></param>
    public void AddItem(E_ItemType itemType, int num)
    {
        //先处理数据
        DataMgr.Instance.AddItem(itemType, num);

        //如果打开了面板，再处理面板
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
        //更新GamePanel金钱
        if(itemType == E_ItemType.Diamond)
        {
            DataMgr.Instance.UpdateMoney();
            UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
        }
    }

    /// <summary>
    /// 移除道具
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="num"></param>
    public bool RemoveItem(E_ItemType itemType, int num)
    {
        //先处理数据
        bool canRemove = DataMgr.Instance.RemoveItem(itemType, num);
        //如果能够取
        if (canRemove)
        {
            //如果打开了面板，再处理面板
            if (isOpenBag)
                UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
            //更新GamePanel金钱
            DataMgr.Instance.UpdateMoney();
            UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
        }
        else
        {
            Debug.Log("道具" + itemType + "不足或没有");
        }
        return canRemove;
    }

    /// <summary>
    /// 更新背包
    /// </summary>
    public void RefreshBag()
    {
        //如果打开了面板，再处理面板
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
        //更新GamePanel金钱
        DataMgr.Instance.UpdateMoney();
        UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
    }

    /// <summary>
    /// 清空道具
    /// </summary>
    public void ClearItem()
    {
        //先处理数据
        DataMgr.Instance.ClearItem();
        //如果打开了面板，再处理面板
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
    }

    /// <summary>
    /// 交换道具在背包中的位置
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    public void SwichItemInBag(Item item1, Item item2)
    {
        //先处理数据
        ItemData temp = DataMgr.Instance.NowBagInfo.slot[item1.pos.ToString()];
        DataMgr.Instance.NowBagInfo.slot[item1.pos.ToString()] = DataMgr.Instance.NowBagInfo.slot[item2.pos.ToString()];
        DataMgr.Instance.NowBagInfo.slot[item2.pos.ToString()] = temp;

        //再处理面板
        UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
    }
    #endregion


}
