using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemObj : MonoBehaviour
{
    public E_ItemType type;
    public int num;

    private UnityAction<KeyCode> inputEvent;

    private void Awake()
    {
        inputEvent = (key) =>
        {
            if(key == KeyCode.E)
            {
                GetMe();
            }
        };
    }

    public void GetMe()
    {
        BagMgr.Instance.AddItem(type, num);
        //显示提示面板
        TipPanel tip = UIMgr.Instance.GetPanel<TipPanel>("TipPanel");
        if (tip != null)
        {
            tip.RewardTip(type, num);
            Destroy(gameObject);
        }
        else
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.RewardTip(type, num);
                Destroy(gameObject);
            });
        }      
    }

    private void OnDestroy()
    {
        UIMgr.Instance.HidePanel("GameTipPanel");
        EventCenter.Instance.RemoveListener<KeyCode>("GetKeyDown", inputEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("按E拾取道具");
                EventCenter.Instance.AddListener<KeyCode>("GetKeyDown", inputEvent);
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.HidePanel("GameTipPanel");
            EventCenter.Instance.RemoveListener<KeyCode>("GetKeyDown", inputEvent);
        }
    }
}
