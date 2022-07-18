using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    public Image botPanel;
    public Image buyItem;
    public Image needItem;
    public Image putItem;

    public Text needItemNum;
    public Text txtBuyNum;

    public ScrollRect sr;

    private ShopInfo nowSelectItem;
    private int buyNum = 1;

    //添加关闭按钮事件监听
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if (btnName == "BtnBack")
        {
            UIMgr.Instance.HidePanel("ShopPanel");
            UIMgr.Instance.HidePanel("BagPanel");
        }
        if(btnName == "BtnAdd")
        {
            buyNum = Mathf.Clamp(buyNum + 1, 1, 99);
            txtBuyNum.text = "X" + buyNum;
        }
        if(btnName == "BtnSub")
        {
            buyNum = Mathf.Clamp(buyNum - 1, 1, 99);
            txtBuyNum.text = "X" + buyNum;
        }
    }

    /// <summary>
    /// 改变放入栏的图片和alpha值
    /// </summary>
    /// <param name="spr"></param>
    /// <param name="alpha"></param>
    public void ChangePutImage(Sprite spr, float alpha)
    {
        putItem.sprite = spr;
        putItem.color = new Color(1, 1, 1, alpha);
    }

    public void SureBuyItem()
    {
        //如果拖入道具不正确，提示拖入正确道具
        if (nowSelectItem == null || putItem.sprite != needItem.sprite)
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("请拖入正确道具?");
            });
        }
        //如果拖入道具正确，提示是否确定购买
        else
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("是否确定购买？", () =>
                {
                    ShopMgr.BuyItem(nowSelectItem, buyNum);
                });
            });
        }
    }

    /// <summary>
    /// 打开或关闭购买面板
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrCloseBuyPanel(bool isOpen)
    {
        if (isOpen)
        {
            buyNum = 1;
            txtBuyNum.text = "X" + buyNum;
        }
        botPanel.gameObject.SetActive(isOpen);
    }

    /// <summary>
    /// 改变购买商品信息
    /// </summary>
    /// <param name="info"></param>
    public void ChangeBuyInfo(ShopInfo info)
    {
        buyItem.sprite = ResMgr.Instance.Load<Sprite>(DataMgr.Instance.itemInfoList[info.itemId].imgRes);
        needItem.sprite = ResMgr.Instance.Load<Sprite>(DataMgr.Instance.itemInfoList[info.moneyId].imgRes);
        needItemNum.text = info.moneyNum.ToString();
        nowSelectItem = info;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        foreach (ShopInfo item in DataMgr.Instance.shopInfoList)
        {
            PoolMgr.Instance.PopObj("UI/ShopItem", (obj) =>
            {
                obj.transform.SetParent(sr.content, false);
                ShopItem shopItem = obj.GetComponent<ShopItem>();
                shopItem.Init(DataMgr.Instance.itemInfoList[item.itemId].imgRes);
                shopItem.shopInfo = item;
            });
        }
        txtBuyNum.text = "X" + buyNum;
        //隐藏购买面板
        OpenOrCloseBuyPanel(false);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.PostEvent("PushShopItem");
        EventCenter.Instance.PostEvent("ExitAIModule");
    }
}
