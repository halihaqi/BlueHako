using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemInfo info = null;//显示道具信息,为空则不显示
    public int pos;//道具栏位置
    public bool isNull = true;

    public Image imgItem;
    public Text txtNum;

    //imgItem参数
    private Vector3 startPos;
    private RectTransform rect;
    private CanvasGroup cg;

    private UnityAction pushEvent;
    private UnityAction itemRaycastEvent;

    private void OnEnable()
    {
        pushEvent = () =>
        {
            PoolMgr.Instance.PushObj("UI/Item", gameObject);
        };
        EventCenter.Instance.AddListener("PushItem", pushEvent);

        itemRaycastEvent = () =>
        {
            cg.blocksRaycasts = !cg.blocksRaycasts;
        };
        EventCenter.Instance.AddListener("ItemRaycast", itemRaycastEvent);
    }

    private void OnDisable()
    {
        ResetMe();
        EventCenter.Instance.RemoveListener("PushItem", pushEvent);
        EventCenter.Instance.RemoveListener("ItemRaycast", itemRaycastEvent);
    }


    public void Init(int num, string imgRes)
    {
        imgItem.gameObject.SetActive(true);
        txtNum.text = num.ToString();
        imgItem.sprite = ResMgr.Instance.Load<Sprite>(imgRes);
        isNull = false;
    }

    public void ResetMe()
    {
        info = null;
        isNull = true;
        imgItem.gameObject.SetActive(false);
    }

    public void ChangeNum(int num)
    {
        txtNum.text = num.ToString();
    }

    public void ChangeImg(string imgRes)
    {
        imgItem.sprite = ResMgr.Instance.Load<Sprite>(imgRes);
    }

    private void Awake()
    {
        //获取imgItem组件
        rect = imgItem.GetComponent<RectTransform>();
        cg = imgItem.GetComponent<CanvasGroup>();

        //拖拽前
        //设置父对象，记录开始坐标，关闭射线遮挡
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.BeginDrag, (eventData) =>
            {
                startPos = rect.localPosition;
                UIMgr.Instance.SetUILayer(rect, E_UI_Layer.System, true);
                EventCenter.Instance.PostEvent("ItemRaycast");
            });

        //添加自定义事件
        //拖拽时 跟随鼠标
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.Drag, (eventData) =>
            {
                Vector3 targetPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle
                    (rect, (eventData as PointerEventData).position, (eventData as PointerEventData).enterEventCamera, out targetPos);
                rect.position = targetPos;
                //测试射线检测
                //print((eventData as PointerEventData).pointerCurrentRaycast.gameObject.name);
            });

        //拖拽结束 判断是移动到该位置 还是回到原处
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.EndDrag, (eventData) =>
            {
                //射线检测当前物体
                GameObject hitObj = (eventData as PointerEventData).pointerCurrentRaycast.gameObject;

                //先返回原点,等待下步操作
                rect.SetParent(transform, true);
                rect.localPosition = startPos;
                EventCenter.Instance.PostEvent("ItemRaycast");

                //如果没有检测到物体,不进行下面操作
                if (hitObj == null)
                    return;

                //根据射到物体的层级选择解决方案
                switch (LayerMask.LayerToName(hitObj.layer))
                {
                    case "Item":
                        BagMgr.Instance.SwichItemInBag(this, hitObj.GetComponent<Item>());
                        break;
                    case "Buy"://拖到购买层级，更新购买框，提示是否确定购买
                        ShopPanel panel = UIMgr.Instance.GetPanel<ShopPanel>("ShopPanel");
                        panel.ChangePutImage(imgItem.sprite, 1);
                        panel.SureBuyItem();
                        break;
                    default:
                        break;
                }


            });
    }
}
