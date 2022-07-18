using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerItem : MonoBehaviour
{
    public Image imgTower;
    public Image imgNeedItem;
    public Text txtName;
    public Text txtLevel;
    public Text txtNeedMoney;
    

    public GameObject chooseObj;
    public GameObject tipObj;

    public bool isChoose = false;
    public TowerInfo info;

    public void ChooseMe()
    {
        chooseObj.SetActive(true);
        tipObj.SetActive(true);
        isChoose = true;
    }

    public void ForgetMe()
    {
        chooseObj.SetActive(false);
        tipObj.SetActive(false);
        isChoose = false;
    }

    /// <summary>
    /// ≥ı ºªØ
    /// </summary>
    /// <param name="info"></param>
    public void Init(TowerInfo info)
    { 
        imgTower.sprite = ResMgr.Instance.Load<Sprite>(info.imgRes);
        imgNeedItem.sprite = ResMgr.Instance.Load<Sprite>(DataMgr.Instance.itemInfoList[info.needId].imgRes);
        txtName.text = info.name;
        txtLevel.text = "LV" + info.level;
        txtNeedMoney.text = "X" + info.money;

        this.info = info;
        ForgetMe();
    }
    
}
