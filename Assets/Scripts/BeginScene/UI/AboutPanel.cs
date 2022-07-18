using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanel : BasePanel
{
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.HidePanel("AboutPanel");
    }
}
