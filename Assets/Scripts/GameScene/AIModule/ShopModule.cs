using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopModule : AIModule
{
    protected override void Awake()
    {
        EnterAIMoudleEvent = (obj) =>
        {
            if (obj.GetComponent<ShopModule>() != null)
            {
                ShopMgr.EnterShop();
            }
        };
        base.Awake();
    }
}
