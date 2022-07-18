using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerPanel : BasePanel
{
    public Transform towerList;

    private TowerPointObj nowTowerPoint = null;
    private List<TowerItem> towerItemList = new List<TowerItem>();

    private UnityAction<KeyCode> inputEvent;
    private int index = 0;


    /// <summary>
    /// 根据当前造塔点初始化UI
    /// </summary>
    /// <param name="nowTowerPoint">当前造塔点</param>
    public void Init(TowerPointObj nowTowerPoint)
    {
        this.nowTowerPoint = nowTowerPoint;
        foreach (TowerInfo info in DataMgr.Instance.towerInfoList)
        {
            //找到所有初级塔并加载出来
            if (info.level == 1)
            {
                TowerInfo targetInfo = info;
                //如果当前已建造了塔，那么搜索到的该塔初级塔更改为当前塔
                if (nowTowerPoint.nowTowerObj != null && info.name == nowTowerPoint.nowTowerObj.info.name)
                    targetInfo = DataMgr.Instance.towerInfoList[nowTowerPoint.nowTowerObj.info.next];

                //加载塔UI
                ResMgr.Instance.LoadAsync<GameObject>("UI/TowerItem", (obj) =>
                {
                    obj.transform.SetParent(towerList, false);
                    TowerItem item = obj.GetComponent<TowerItem>();
                    item.Init(targetInfo);
                    towerItemList.Add(item);
                    if (towerItemList.Count == 1)
                        towerItemList[0].ChooseMe();
                });
            }
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        inputEvent = (key) =>
        {
            if (key == KeyCode.Q)
            {
                index = index - 1 < 0 ? towerItemList.Count - 1 : index - 1;
                for (int i = 0; i < towerItemList.Count; i++)
                {
                    if (i == index)
                        towerItemList[i].ChooseMe();
                    else
                        towerItemList[i].ForgetMe();
                }
            }
            if (key == KeyCode.E)
            {
                index = index + 1 > towerItemList.Count - 1 ? 0 : index + 1;
                for (int i = 0; i < towerItemList.Count; i++)
                {
                    if (i == index)
                        towerItemList[i].ChooseMe();
                    else
                        towerItemList[i].ForgetMe();
                }
            }
            if (key == KeyCode.Tab)
            {
                //先判断是否强化满
                if (nowTowerPoint.nowTowerObj?.info.next == 0)
                {
                    UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                    {
                        panel.ChangeTipInfo("已强化至最高等级");
                    });
                    return;
                }

                //记录要升级到的目标信息
                int targetId = towerItemList[index].info.id;
                TowerInfo targetInfo = DataMgr.Instance.towerInfoList[targetId];

                //判断是否满足建造
                if(MainTowerObj.Instance.NowMoney >= targetInfo.money &&
                    DataMgr.Instance.GetBagItemNum((E_ItemType)targetInfo.needId) > 0)
                {
                    DataMgr.Instance.RemoveItem((E_ItemType)targetInfo.needId, 1);
                    MainTowerObj.Instance.UpdateMoney(-targetInfo.money);
                }
                else
                {
                    UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                    {
                        panel.ChangeTipInfo("道具不足，无法建造QAQ");
                    });
                    return;
                }

                //开始建造和更新面板
                nowTowerPoint.BuildTower(targetInfo);
                towerItemList[index].Init(DataMgr.Instance.towerInfoList[targetInfo.next]);
                towerItemList[index].ChooseMe();

                //如果是新建或重建
                if (targetInfo.name != nowTowerPoint.nowTowerObj?.info.name)
                {
                    //将除选中UI外其他恢复初始
                    for (int i = 0; i < towerItemList.Count; i++)
                    {
                        if (i != index)
                        {
                            foreach (TowerInfo info in DataMgr.Instance.towerInfoList)
                            {
                                if (info.name == towerItemList[i].info.name && info.level == 1)
                                    towerItemList[i].Init(info);
                            }
                        }
                    }
                }
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
