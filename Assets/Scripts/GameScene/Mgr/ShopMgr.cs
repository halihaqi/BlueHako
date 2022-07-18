using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMgr : Singleton<ShopMgr>
{
    /// <summary>
    /// �����̵�
    /// </summary>
    public static void EnterShop()
    {
        UIMgr.Instance.HideAllPanel("GamePanel");
        UIMgr.Instance.ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Bot);
        UIMgr.Instance.ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Mid);
    }

    /// <summary>
    /// �뿪�̵�
    /// </summary>
    public static void ExitShop()
    {
        UIMgr.Instance.HidePanel("BagPanel");
        UIMgr.Instance.HidePanel("ShopPanel");
    }

    /// <summary>
    /// �򿪻�رչ������
    /// </summary>
    /// <param name="isOpen"></param>
    public static void OpenOrCloseBuyPanel(bool isOpen)
    {
        ShopPanel panel = UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel");
        panel.OpenOrCloseBuyPanel(isOpen);
        panel.ChangePutImage(null, 0);
    }

    /// <summary>
    /// ���¹������
    /// </summary>
    /// <param name="info"></param>
    public static void UpdateBuyPanel(ShopInfo info)
    {
        UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel").ChangeBuyInfo(info);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="info"></param>
    /// <param name="num"></param>
    public static void BuyItem(ShopInfo info, int num)
    {
        if(DataMgr.Instance.BuyItem(info.id, num))
        {
            UIMgr.Instance.GetPanel<BagPanel>("BagPanel").UpdateItem();
            UIMgr.Instance.GetPanel<GamePanel>("GamePanel").UpdateMoney();
            UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel").OpenOrCloseBuyPanel(false);
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("����ɹ���");
            });
        }
        else
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("���߲���,����ʧ��QAQ");
            });
        }

    }
}
