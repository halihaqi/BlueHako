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
    /// ���ݵ�ǰ�������ʼ��UI
    /// </summary>
    /// <param name="nowTowerPoint">��ǰ������</param>
    public void Init(TowerPointObj nowTowerPoint)
    {
        this.nowTowerPoint = nowTowerPoint;
        foreach (TowerInfo info in DataMgr.Instance.towerInfoList)
        {
            //�ҵ����г����������س���
            if (info.level == 1)
            {
                TowerInfo targetInfo = info;
                //�����ǰ�ѽ�����������ô�������ĸ�������������Ϊ��ǰ��
                if (nowTowerPoint.nowTowerObj != null && info.name == nowTowerPoint.nowTowerObj.info.name)
                    targetInfo = DataMgr.Instance.towerInfoList[nowTowerPoint.nowTowerObj.info.next];

                //������UI
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
                //���ж��Ƿ�ǿ����
                if (nowTowerPoint.nowTowerObj?.info.next == 0)
                {
                    UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                    {
                        panel.ChangeTipInfo("��ǿ������ߵȼ�");
                    });
                    return;
                }

                //��¼Ҫ��������Ŀ����Ϣ
                int targetId = towerItemList[index].info.id;
                TowerInfo targetInfo = DataMgr.Instance.towerInfoList[targetId];

                //�ж��Ƿ����㽨��
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
                        panel.ChangeTipInfo("���߲��㣬�޷�����QAQ");
                    });
                    return;
                }

                //��ʼ����͸������
                nowTowerPoint.BuildTower(targetInfo);
                towerItemList[index].Init(DataMgr.Instance.towerInfoList[targetInfo.next]);
                towerItemList[index].ChooseMe();

                //������½����ؽ�
                if (targetInfo.name != nowTowerPoint.nowTowerObj?.info.name)
                {
                    //����ѡ��UI�������ָ���ʼ
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
