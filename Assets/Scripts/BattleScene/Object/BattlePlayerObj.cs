using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerObj : PlayerObj
{
    protected override void Awake()
    {
        base.Awake();
        EventCenter.Instance.RemoveListener<KeyCode>("GetKeyDown", inputEvent);
        inputEvent = (key) =>
        {
            if (key == KeyCode.Escape)
            {
                UIMgr.Instance.ShowPanel<ExitPanel>("ExitPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.BattleSceneMode();
                });
            }
        };
        EventCenter.Instance.AddListener<KeyCode>("GetKeyDown", inputEvent);
    }
}
