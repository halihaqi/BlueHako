using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRolePanel : BasePanel
{
    private int nowRoleId = 0;
    public RoleObj nowRoleObj;
    private Transform bornPos;
    private bool isChanging = false;

    //按钮事件
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "BtnBack":
                //返回开始面板
                UIMgr.Instance.HidePanel("ChooseRolePanel");
                Begin_Cam.Instance.MoveBack();
                UIMgr.Instance.ShowPanel<BeginPanel>("BeginPanel", E_UI_Layer.Bot);
                nowRoleObj.DestroyMe(null);
                break;
            case "BtnChange":
                //如果正在换人，按钮不执行逻辑
                if (isChanging)
                    return;
                //维护索引
                //如果超过最后一个就变为第一个
                nowRoleId = nowRoleId + 1 > DataMgr.Instance.roleInfoList.Count - 1 ? 0 : nowRoleId + 1;
                //切换人物
                ChangeRole(nowRoleId);
                break;
            case "BtnOk":
                //显示存档面板
                UIMgr.Instance.ShowPanel<SaveLoadPanel>("SaveLoadPanel", E_UI_Layer.Bot, (panel) =>
                {
                    //改变SaveLoadPanel的当前角色，用于创建新存档
                    panel.roleId = nowRoleId;
                    panel.slType = E_SaveOrLoad.Save;
                });
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 切换人物逻辑，控制场景中人物的出生与销毁
    /// </summary>
    /// <param name="id"></param>
    private void ChangeRole(int id)
    {
        //正在换人
        isChanging = true;

        //如果场景中有角色
        if (nowRoleObj != null)
        {
            //先销毁当前角色
            nowRoleObj.DestroyMe(() =>
            {
                //销毁结束再创建新的角色
                //获得角色数据
                RoleInfo info = DataMgr.Instance.roleInfoList[id];
                //在BornPos下生成物体
                ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
                {
                    //设置出生点
                    obj.transform.SetParent(bornPos, false);
                    //替换当前角色
                    nowRoleObj = obj.AddComponent<RoleObj>();
                    nowRoleObj.DynamicMode();
                    //结束换人
                    isChanging = false;
                });
            });
        }
        //如果场景中没有角色
        else
        {
            //直接创建新角色
            //获得角色数据
            RoleInfo info = DataMgr.Instance.roleInfoList[id];
            //在BornPos下生成物体
            ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
            {
                //设置出生点
                obj.transform.SetParent(bornPos, false);
                //替换当前角色
                nowRoleObj = obj.AddComponent<RoleObj>();
                nowRoleObj.DynamicMode();

                //结束换人
                isChanging = false;
            });
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //一开始找到出生点，初始化角色
        bornPos = GameObject.Find("BornPos").transform;
        ChangeRole(nowRoleId);

        //添加进入游戏时的事件监听
        customEvent = () => { UIMgr.Instance.HidePanel("ChooseRolePanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //移除进入游戏时的事件监听
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }
}
