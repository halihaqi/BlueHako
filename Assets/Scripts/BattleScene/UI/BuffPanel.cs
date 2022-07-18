using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffPanel : BasePanel
{
    public List<GameObject> tipList;
    private int index = 0;

    private UnityAction<KeyCode> inputEvent;

    public override void ShowMe()
    {
        base.ShowMe();
        foreach (GameObject tip in tipList)
        {
            tip.SetActive(false);
        }
        tipList[0].SetActive(true);
        inputEvent = (key) =>
        {
            if (key == KeyCode.Q)
            {
                tipList[index].SetActive(false);
                index = index - 1 < 0 ? tipList.Count - 1 : index - 1;
                tipList[index].SetActive(true);
            }
            if (key == KeyCode.E)
            {
                tipList[index].SetActive(false);
                index = index + 1 > tipList.Count - 1 ? 0 : index + 1;
                tipList[index].SetActive(true);
            }
            if (key == KeyCode.Tab)
            {
                string tipStr = "";
                switch (index)
                {
                    case 0://增加生命,增加五分之一的血量
                        MainTowerObj.Instance.Hp += MainTowerObj.Instance.MaxHp / 5;
                        tipStr = "主塔恢复生命" + MainTowerObj.Instance.MaxHp / 5 + "点！";
                        break;
                    case 1://提升血限,提高300点生命
                        MainTowerObj.Instance.MaxHp += 300;
                        tipStr = "主塔血量提升" + 300 + "点！";
                        break;
                    case 2://提高防御,提高5点防御
                        MainTowerObj.Instance.Def += 5;
                        tipStr = "主塔防御提高" + 5 + "点！";
                        break;
                    default:
                        break;
                }
                MainTowerObj.Instance.UpdateHp(MainTowerObj.Instance.Hp, MainTowerObj.Instance.MaxHp);
                UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                {
                    panel.ChangeTipInfo(tipStr);
                });
            }

        };
        EventCenter.Instance.AddListener<KeyCode>("GetKeyDown", inputEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveListener<KeyCode>("GetKeyDown", inputEvent);
    }
}
