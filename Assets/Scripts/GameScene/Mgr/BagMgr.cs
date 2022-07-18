using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������,
/// ��DataMgr.Instance.itemInfoList[i]һһ��Ӧ
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

    #region ������������
    /// <summary>
    /// �򿪻�رձ���
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
    /// ��ӵ���
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="num"></param>
    public void AddItem(E_ItemType itemType, int num)
    {
        //�ȴ�������
        DataMgr.Instance.AddItem(itemType, num);

        //���������壬�ٴ������
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
        //����GamePanel��Ǯ
        if(itemType == E_ItemType.Diamond)
        {
            DataMgr.Instance.UpdateMoney();
            UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
        }
    }

    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="num"></param>
    public bool RemoveItem(E_ItemType itemType, int num)
    {
        //�ȴ�������
        bool canRemove = DataMgr.Instance.RemoveItem(itemType, num);
        //����ܹ�ȡ
        if (canRemove)
        {
            //���������壬�ٴ������
            if (isOpenBag)
                UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
            //����GamePanel��Ǯ
            DataMgr.Instance.UpdateMoney();
            UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
        }
        else
        {
            Debug.Log("����" + itemType + "�����û��");
        }
        return canRemove;
    }

    /// <summary>
    /// ���±���
    /// </summary>
    public void RefreshBag()
    {
        //���������壬�ٴ������
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
        //����GamePanel��Ǯ
        DataMgr.Instance.UpdateMoney();
        UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
    }

    /// <summary>
    /// ��յ���
    /// </summary>
    public void ClearItem()
    {
        //�ȴ�������
        DataMgr.Instance.ClearItem();
        //���������壬�ٴ������
        if (isOpenBag)
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
    }

    /// <summary>
    /// ���������ڱ����е�λ��
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    public void SwichItemInBag(Item item1, Item item2)
    {
        //�ȴ�������
        ItemData temp = DataMgr.Instance.NowBagInfo.slot[item1.pos.ToString()];
        DataMgr.Instance.NowBagInfo.slot[item1.pos.ToString()] = DataMgr.Instance.NowBagInfo.slot[item2.pos.ToString()];
        DataMgr.Instance.NowBagInfo.slot[item2.pos.ToString()] = temp;

        //�ٴ������
        UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
    }
    #endregion


}
