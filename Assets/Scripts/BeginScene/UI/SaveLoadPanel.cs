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
        //切换存档或读档图标
        imgSave.gameObject.SetActive(slType == E_SaveOrLoad.Save);
        imgLoad.gameObject.SetActive(slType == E_SaveOrLoad.Load);
        //加载存档
        InitFiles();

        //添加进入游戏时的事件监听
        customEvent = () => { UIMgr.Instance.HidePanel("SaveLoadPanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //移除进入游戏时的事件监听
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }

    //因为添加点击事件时存档按钮还没生成，所以只有退出按钮
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        UIMgr.Instance.HidePanel("SaveLoadPanel");
    }

    /// <summary>
    /// 点击按钮后，加载和进入游戏的逻辑
    /// </summary>
    private void LoadRes()
    {
        SceneMgr.Instance.LoadSceneAsyncPro("GameScene", () =>
        {
            //先通知进入游戏，让监听的面板全部隐藏
            EventCenter.Instance.PostEvent("EnterGame");
            //初始化玩家
            GameMgr.Instance.InitPlayer();
            //创建GameUI
            UIMgr.Instance.ShowPanel<GamePanel>("GamePanel", E_UI_Layer.Bot, (panel) =>
            {
                panel.Init(DataMgr.Instance.NowPlayerInfo);
            });
        });
    }

    /// <summary>
    /// 初始化存档
    /// </summary>
    private void InitFiles()
    {
        //显示时动态生成存档按钮
        Dictionary<string, PlayerInfo> playerList = DataMgr.Instance.playerData.playerList;
        //添加十个存档按钮
        for (int i = 0; i < 10; i++)
        {
            //index记录迭代变量
            int index = i;

            //如果index在playerList范围内，就加载BtnFile，否则加载BtnNull
            string path = (index <= playerList.Count - 1) ? "UI/BtnFile" : "UI/BtnNull";
            ResMgr.Instance.LoadAsync<GameObject>(path, (obj) =>
            {
                //1.改变基础信息
                obj.name = path + index;
                obj.transform.SetParent(sr.content, false);

                //2.改变按钮面板,如果有BtnFile才改变               
                BtnFile item;
                if (obj.TryGetComponent(out item))
                    item.ChangeInfo(playerList[index.ToString()]);

                //3.添加按钮存或读的事件监听
                Button btn = obj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    switch (slType)
                    {
                        case E_SaveOrLoad.Save:
                            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                            {
                                panel.ChangeTipInfo((index <= playerList.Count - 1) ? "是否覆盖存档？" : "是否创建新存档？", () =>
                                {
                                    //创建存档，将Data中的nowPlayerInfo改为该存档
                                    DataMgr.Instance.CreateNewPlayerInfo(index, roleId);
                                    //加载并进入
                                    LoadRes();
                                });
                            });
                            break;
                        case E_SaveOrLoad.Load:
                            //如果是空存档，不添加点击事件
                            if (obj.name == "UI/BtnNull" + index)
                                return;

                            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
                            {
                                panel.ChangeTipInfo("是否选择该存档？", () =>
                                {
                                    //加载存档，将Data中的nowPlayerInfo改为该存档
                                    DataMgr.Instance.LoadPlayerInfo(index);
                                    //加载并进入
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
