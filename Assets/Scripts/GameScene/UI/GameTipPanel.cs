using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTipPanel : BasePanel
{
    public Text txtTip;
    
    public void ChangeInfo(string tip)
    {
        txtTip.text = tip;
    }
}
