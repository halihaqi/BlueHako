using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum E_SaveOrLoad
{
    Save,Load
}

public class SaveLoadPanel : BasePanel
{
    [HideInInspector]
    public int roleId;
    [HideInInspector]
    public E_SaveOrLoad slType = E_SaveOrLoad.Save;

    public Image imgSave;
    public Image imgLoad;
    public ScrollRect sr;

    public override void ShowMe()
    {
        base.ShowMe();
        //�л��浵�����ͼ��
        imgSave.gameObject.SetActive(slType == E_SaveOrLoad.Save);
        imgLoad.gameObject.SetActive(slType == E_SaveOrLoad.Load);
        //���ش浵
        InitFiles();

        //��ӽ�����Ϸʱ���¼�����
        customEvent = () => { UIMgr.Instance.HidePanel("SaveLoadPanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //�Ƴ�������Ϸʱ���¼�����
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }

    //��Ϊ��ӵ���¼�ʱ�浵��ť��û���ɣ�����ֻ���˳���ť
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.HidePanel("SaveLoadPanel");
    }

    /// <summary>
    /// �����ť�󣬼��غͽ�����Ϸ���߼�
    /// </summary>
    private void LoadRes()
    {
        SceneMgr.Instance.LoadSceneAsyncPro("GameScene", () =>
        {
            //��֪ͨ������Ϸ���ü��������ȫ������
            EventCenter.Instance.PostEvent("EnterGame");
            //��ʼ�����
            GameMgr.Instance.InitPlayer();
            //����GameUI
            UIMgr.Instance.ShowPanel<GamePanel>("GamePanel", E_UI_Layer.Bot, (panel) =>
            {
                panel.Init(DataMgr.Instance.NowPlayerInfo);
            });
        });
    }

    /// <summary>
    /// ��ʼ���浵
    /// </summary>
    private void InitFiles()
    {
        //��ʾʱ��̬���ɴ浵��ť
        Dictionary<string, PlayerInfo> playerList = DataMgr.Instance.playerData.playerList;
        //���ʮ���浵��ť
        for (int i = 0; i < 10; i++)
        {
            //index��¼��������
            int index = i;

            //���index��playerList��Χ�ڣ��ͼ���BtnFile���������BtnNull
            string path = (index <= playerList.Count - 1) ? "UI/BtnFile" : "UI/BtnNull";
            ResMgr.Instance.LoadAsync<GameObject>(path, (obj) =>
            {
                //1.�ı������Ϣ
                obj.name = path + index;
                obj.transform.SetParent(sr.content, false);

                //2.�ı䰴ť���,�����BtnFile�Ÿı�               
                BtnFile item;
                if (obj.TryGetComponent(out item))
                    item.ChangeInfo(playerList[index.ToString()]);

                //3.��Ӱ�ť�������¼�����
                Button btn = obj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    switch (slType)
                    {
                        case E_SaveOrLoad.Save:
                            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                            {
                                panel.ChangeTipInfo((index <= playerList.Count - 1) ? "�Ƿ񸲸Ǵ浵��" : "�Ƿ񴴽��´浵��", () =>
                                {
                                    //�����浵����Data�е�nowPlayerInfo��Ϊ�ô浵
                                    DataMgr.Instance.CreateNewPlayerInfo(index, roleId);
                                    //���ز�����
                                    LoadRes();
                                });
                            });
                            break;
                        case E_SaveOrLoad.Load:
                            //����ǿմ浵������ӵ���¼�
                            if (obj.name == "UI/BtnNull" + index)
                                return;

                            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                            {
                                panel.ChangeTipInfo("�Ƿ�ѡ��ô浵��", () =>
                                {
                                    //���ش浵����Data�е�nowPlayerInfo��Ϊ�ô浵
                                    DataMgr.Instance.LoadPlayerInfo(index);
                                    //���ز�����
                                    LoadRes();
                                });
                            });
                            break;
                        default:
                            break;
                    }
                });
            });

        }
    }
    
}
