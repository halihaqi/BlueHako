using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    public ScrollRect sr;

    //����item�б�
    private List<Item> itemList = new List<Item>(DataMgr.Instance.NowBagInfo.bagCapacity);

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.HidePanel("BagPanel");
        UIMgr.Instance.HidePanel("ShopPanel");
    }

    /// <summary>
    /// ��ʼ���հ׵�����,�Ӷ����ȡ��
    /// ����ʼ��
    /// </summary>
    public void InitItemBar()
    {
        //�ȸ��ݱ������������ʾ�հ׵�����
        //��Ϊ���첽���أ�����Ҫ��ȫ���������ٸ���
        //�˴��бհ�����
        for (int i = 0; i < DataMgr.Instance.NowBagInfo.bagCapacity; i++)
        {
            int index = i;
            PoolMgr.Instance.PopObj("UI/Item", (obj) =>
            {
                obj.transform.SetParent(sr.content, false);
                Item item = obj.GetComponent<Item>();
                //��ʼ��pos����ӵ��б�
                item.pos = index;
                itemList.Add(item);

                //���������һ������
                if (index == DataMgr.Instance.NowBagInfo.bagCapacity - 1)
                    UpdateItem();
            });
        }
    }

    /// <summary>
    /// ���µ�����
    /// </summary>
    public void UpdateItem()
    {
        //��Ϊ�����ڴ���ʱ�Ѿ��г�ʼֵ
        //����ѭ��������ÿһ��ֵ���Ǻ�itemList�е�ֵһһ��Ӧ��
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
        //��ʾʱ���ص���������ʼ��
        InitItemBar();
    }

    public override void HideMe()
    {
        base.HideMe();
        //����ʱ�㲥Push���ߵ��������,ÿһ��Item����
        EventCenter.Instance.PostEvent("PushItem");
    }
}
