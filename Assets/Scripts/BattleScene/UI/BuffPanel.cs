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
                    case 0://��������,�������֮һ��Ѫ��
                        MainTowerObj.Instance.Hp += MainTowerObj.Instance.MaxHp / 5;
                        tipStr = "�����ָ�����" + MainTowerObj.Instance.MaxHp / 5 + "�㣡";
                        break;
                    case 1://����Ѫ��,���300������
                        MainTowerObj.Instance.MaxHp += 300;
                        tipStr = "����Ѫ������" + 300 + "�㣡";
                        break;
                    case 2://��߷���,���5�����
                        MainTowerObj.Instance.Def += 5;
                        tipStr = "�����������" + 5 + "�㣡";
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
