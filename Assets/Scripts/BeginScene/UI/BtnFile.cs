using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnFile : MonoBehaviour
{
    public Image imgComplete;
    public Image imgRole;
    public Text txtName;
    public Text txtMoney;
    public Text txtTime;
    public Text txtComplete;

    public void ChangeInfo(PlayerInfo info)
    {
        RoleInfo roleInfo = DataMgr.Instance.roleInfoList[info.roleId];
        imgRole.sprite = ResMgr.Instance.Load<Sprite>(roleInfo.imgRes);
        txtName.text = roleInfo.name;
        txtMoney.text = "X " + info.money.ToString();
        txtTime.text = info.time.ToTime();
        txtComplete.text = info.complete.ToString() + "%";
        imgComplete.sprite = ResMgr.Instance.Load<Sprite>("RankImg/Rank" + ((int)info.complete / 25));
    }
}
