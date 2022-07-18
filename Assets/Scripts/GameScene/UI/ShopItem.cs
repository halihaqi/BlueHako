using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShopInfo shopInfo;
    public Image imgItem;

    private UnityAction pushEvent;

    private void OnEnable()
    {
        pushEvent = () =>
        {
            PoolMgr.Instance.PushObj("UI/ShopItem", gameObject);
        };
        EventCenter.Instance.AddListener("PushShopItem", pushEvent);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener("PushShopItem", pushEvent);
    }

    public void Init(string imgRes)
    {
        imgItem.gameObject.SetActive(true);
        imgItem.sprite = ResMgr.Instance.Load<Sprite>(imgRes);
    }

    public void ResetMe()
    {
        imgItem.gameObject.SetActive(false);
    }

    private void Awake()
    {
        UIMgr.AddCustomListener
            (imgItem, UnityEngine.EventSystems.EventTriggerType.PointerClick, (eventData) =>
            {
                //�ȴ򿪹������
                ShopMgr.OpenOrCloseBuyPanel(true);
                //�ٸ��������Ϣ
                ShopMgr.UpdateBuyPanel(shopInfo);
            });
    }
}
