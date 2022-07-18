using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemInfo info = null;//��ʾ������Ϣ,Ϊ������ʾ
    public int pos;//������λ��
    public bool isNull = true;

    public Image imgItem;
    public Text txtNum;

    //imgItem����
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
        //��ȡimgItem���
        rect = imgItem.GetComponent<RectTransform>();
        cg = imgItem.GetComponent<CanvasGroup>();

        //��קǰ
        //���ø����󣬼�¼��ʼ���꣬�ر������ڵ�
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.BeginDrag, (eventData) =>
            {
                startPos = rect.localPosition;
                UIMgr.Instance.SetUILayer(rect, E_UI_Layer.System, true);
                EventCenter.Instance.PostEvent("ItemRaycast");
            });

        //����Զ����¼�
        //��קʱ �������
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.Drag, (eventData) =>
            {
                Vector3 targetPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle
                    (rect, (eventData as PointerEventData).position, (eventData as PointerEventData).enterEventCamera, out targetPos);
                rect.position = targetPos;
                //�������߼��
                //print((eventData as PointerEventData).pointerCurrentRaycast.gameObject.name);
            });

        //��ק���� �ж����ƶ�����λ�� ���ǻص�ԭ��
        UIMgr.AddCustomListener
            (imgItem, EventTriggerType.EndDrag, (eventData) =>
            {
                //���߼�⵱ǰ����
                GameObject hitObj = (eventData as PointerEventData).pointerCurrentRaycast.gameObject;

                //�ȷ���ԭ��,�ȴ��²�����
                rect.SetParent(transform, true);
                rect.localPosition = startPos;
                EventCenter.Instance.PostEvent("ItemRaycast");

                //���û�м�⵽����,�������������
                if (hitObj == null)
                    return;

                //�����䵽����Ĳ㼶ѡ��������
                switch (LayerMask.LayerToName(hitObj.layer))
                {
                    case "Item":
                        BagMgr.Instance.SwichItemInBag(this, hitObj.GetComponent<Item>());
                        break;
                    case "Buy"://�ϵ�����㼶�����¹������ʾ�Ƿ�ȷ������
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
