using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBattleModule : AIModule
{
    protected override void Awake()
    {
        EnterAIMoudleEvent = (obj) =>
        {
            if (obj.GetComponent<ChooseBattleModule>() != null)
            {
                UIMgr.Instance.HideAllPanel("GamePanel");
                UIMgr.Instance.ShowPanel<ChooseBattlePanel>("ChooseBattlePanel", E_UI_Layer.Bot);
            }
        };
        base.Awake();
    }
}
