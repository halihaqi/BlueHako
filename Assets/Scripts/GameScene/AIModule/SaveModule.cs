using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveModule : AIModule
{
    protected override void Awake()
    {
        EnterAIMoudleEvent = (obj) =>
        {
            if (obj.GetComponent<SaveModule>() != null)
            {
                UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.Bot, (panel) =>
                {
                    panel.ChangeTipInfo("�Ƿ񱣴���ļ���?", () =>
                    {
                        DataMgr.Instance.SavePlayerData();
                        UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
                        {
                            panel.ChangeInfo("����ɹ�!");
                        });
                    });
                });
            }
        };
        base.Awake();
    }
}
