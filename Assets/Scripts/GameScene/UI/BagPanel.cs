using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    public ScrollRect sr;

    //背包item列表
    private List<Item> itemList = new List<Item>(DataMgr.Instance.NowBagInfo.bagCapacity);

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.HidePanel("BagPanel");
        UIMgr.Instance.HidePanel("ShopPanel");
    }

    /// <summary>
    /// 初始化空白道具栏,从对象池取出
    /// 并初始化
    /// </summary>
    public void InitItemBar()
    {
        //先根据背包最大容量显示空白道具栏
        //因为是异步加载，所以要等全部加载完再更新
        //此处有闭包问题
        for (int i = 0; i < DataMgr.Instance.NowBagInfo.bagCapacity; i++)
        {
            int index = i;
            PoolMgr.Instance.PopObj("UI/Item", (obj) =>
            {
                obj.transform.SetParent(sr.content, false);
                Item item = obj.GetComponent<Item>();
                //初始化pos并添加到列表
                item.pos = index;
                itemList.Add(item);

                //加载完最后一个更新
                if (index == DataMgr.Instance.NowBagInfo.bagCapacity - 1)
                    UpdateItem();
            });
        }
    }

    /// <summary>
    /// 更新道具栏
    /// </summary>
    public void UpdateItem()
    {
        //因为背包在创建时已经有初始值
        //所以循环背包的每一个值都是和itemList中的值一一对应的
        Dictionary<string, ItemData> slot = DataMgr.Instance.NowBagInfo.slot;

        foreach(string id in slot.Keys)
        {
            itemList[int.Parse(id)].info = slot[id].info;
            if (slot[id].info != null)
                itemList[int.Parse(id)].Init(slot[id].num, slot[id].info.imgRes);
            else
                itemList[int.Parse(id)].ResetMe();
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //显示时加载道具栏并初始化
        InitItemBar();
    }

    public override void HideMe()
    {
        base.HideMe();
        //隐藏时广播Push道具到对象池里,每一个Item监听
        EventCenter.Instance.PostEvent("PushItem");
    }
}
