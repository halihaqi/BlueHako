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

    //��ӹرհ�ť�¼�����
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
    /// �ı��������ͼƬ��alphaֵ
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
        //���������߲���ȷ����ʾ������ȷ����
        if (nowSelectItem == null || putItem.sprite != needItem.sprite)
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("��������ȷ����?");
            });
        }
        //������������ȷ����ʾ�Ƿ�ȷ������
        else
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("�Ƿ�ȷ������", () =>
                {
                    ShopMgr.BuyItem(nowSelectItem, buyNum);
                });
            });
        }
    }

    /// <summary>
    /// �򿪻�رչ������
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
    /// �ı乺����Ʒ��Ϣ
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
        //���ع������
        OpenOrCloseBuyPanel(false);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.PostEvent("PushShopItem");
        EventCenter.Instance.PostEvent("ExitAIModule");
    }
}
