using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMgr : Singleton<ShopMgr>
{
    /// <summary>
    /// 进入商店
    /// </summary>
    public static void EnterShop()
    {
        UIMgr.Instance.HideAllPanel("GamePanel");
        UIMgr.Instance.ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Bot);
        UIMgr.Instance.ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Mid);
    }

    /// <summary>
    /// 离开商店
    /// </summary>
    public static void ExitShop()
    {
        UIMgr.Instance.HidePanel("BagPanel");
        UIMgr.Instance.HidePanel("ShopPanel");
    }

    /// <summary>
    /// 打开或关闭购买面板
    /// </summary>
    /// <param name="isOpen"></param>
    public static void OpenOrCloseBuyPanel(bool isOpen)
    {
        ShopPanel panel = UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel");
        panel.OpenOrCloseBuyPanel(isOpen);
        panel.ChangePutImage(null, 0);
    }

    /// <summary>
    /// 更新购买面板
    /// </summary>
    /// <param name="info"></param>
    public static void UpdateBuyPanel(ShopInfo info)
    {
        UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel").ChangeBuyInfo(info);
    }

    /// <summary>
    /// 购买道具
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
                panel.ChangeTipInfo("购买成功！");
            });
        }
        else
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("道具不足,购买失败QAQ");
            });
        }

    }
}
