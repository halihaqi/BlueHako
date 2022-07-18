using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBattlePanel : BasePanel
{
    public Image imgBattle;
    public Image imgTower;
    public Image imgHard;
    public Text txtName;
    public Text txtTip;
    public Text txtMoney;
    public Text txtHp;
    public GameObject completeObj;

    private int nowSceneId = 0;

    /// <summary>
    /// 根据场景id更新面板
    /// </summary>
    /// <param name="id"></param>
    private void UpdatePanel(int id)
    {
        SceneInfo info = DataMgr.Instance.sceneInfoList[id];
        imgBattle.sprite = ResMgr.Instance.Load<Sprite>(info.imgRes);
        imgTower.sprite = ResMgr.Instance.Load<Sprite>(info.towerImgRes);
        imgHard.sprite = ResMgr.Instance.Load<Sprite>("ItemImg/Img_Hard" + info.hard);
        txtName.text = info.name;
        txtTip.text = info.tip;
        txtMoney.text = ": " + info.money;
        txtHp.text = "HP: " + info.towerHp;
        completeObj.SetActive(DataMgr.Instance.NowPlayerInfo.battleCompleteList.Contains(id));
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanel(nowSceneId);
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "BtnSure":
                DataMgr.Instance.NowSceneInfo = DataMgr.Instance.sceneInfoList[nowSceneId];
                SceneMgr.Instance.LoadSceneAsyncPro(DataMgr.Instance.NowSceneInfo.sceneName, () =>
                {
                    GameMgr.Instance.InitPlayer(true);
                    MainTowerObj.Instance.InitMainTower(DataMgr.Instance.NowSceneInfo);
                });
                UIMgr.Instance.HideAllPanel();
                break;
            case "BtnQuit":
                UIMgr.Instance.HidePanel("ChooseBattlePanel");
                break;
            case "BtnLeft":
                nowSceneId = nowSceneId - 1 < 0 ? DataMgr.Instance.sceneInfoList.Count - 1 : nowSceneId - 1;
                UpdatePanel(nowSceneId);
                break;
            case "BtnRight":
                nowSceneId = nowSceneId + 1 > DataMgr.Instance.sceneInfoList.Count - 1 ? 0 : nowSceneId + 1;
                UpdatePanel(nowSceneId);
                break;
            default:
                break;
        }
    }
}
