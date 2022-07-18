using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatObj : MonoBehaviour
{
    private UnityAction<KeyCode> inputEvent;
    private void Awake()
    {
        inputEvent = (key) =>
        {
            if (key == KeyCode.E)
            {
                DataMgr.Instance.AddItem(E_ItemType.Cat, 1);
                UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.ChangeTipInfo("�ٺٺ١�����Сè��(����ˮ)");
                });
                Destroy(gameObject);
            }
        };
    }

    private void OnDestroy()
    {
        UIMgr.Instance.HidePanel("GameTipPanel");
        EventCenter.Instance.RemoveListener("GetKeyDown", inputEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("��EץȡСè");
            });
            EventCenter.Instance.AddListener("GetKeyDown", inputEvent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.HidePanel("GameTipPanel");
            EventCenter.Instance.RemoveListener("GetKeyDown", inputEvent);
        }
    }
}
