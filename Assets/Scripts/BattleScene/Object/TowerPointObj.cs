using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPointObj : MonoBehaviour
{
    [HideInInspector]
    public TowerObj nowTowerObj;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="info"></param>
    public void BuildTower(TowerInfo info)
    {
        if (nowTowerObj != null)
            Destroy(nowTowerObj.gameObject);
        ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
        {
            obj.transform.SetParent(transform, false);
            TowerObj towerObj = obj.GetComponent<TowerObj>();
            towerObj.Init(info);
            nowTowerObj = towerObj;
        });
    }

    /// <summary>
    /// ������
    /// </summary>
    public void UpdateTower()
    {
        if (nowTowerObj == null)
            return;
        if (nowTowerObj.info.next == 0)
            return;
        TowerInfo nextInfo = DataMgr.Instance.towerInfoList[nowTowerObj.info.next];
        Destroy(nowTowerObj.gameObject);
        ResMgr.Instance.LoadAsync<GameObject>(nextInfo.res, (obj) =>
        {
            TowerObj towerObj = obj.GetComponent<TowerObj>();
            nowTowerObj = towerObj;
            towerObj.Init(nextInfo);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("��Q��Eѡ����Ҫ�����������Tab����");
            });
            UIMgr.Instance.ShowPanel<TowerPanel>("TowerPanel", E_UI_Layer.Bot, (panel) =>
            {
                panel.Init(this);
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIMgr.Instance.HidePanel("GameTipPanel");
            UIMgr.Instance.HidePanel("TowerPanel");
        }
    }
}
