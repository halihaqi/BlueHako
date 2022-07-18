using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRolePanel : BasePanel
{
    private int nowRoleId = 0;
    public RoleObj nowRoleObj;
    private Transform bornPos;
    private bool isChanging = false;

    //��ť�¼�
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "BtnBack":
                //���ؿ�ʼ���
                UIMgr.Instance.HidePanel("ChooseRolePanel");
                Begin_Cam.Instance.MoveBack();
                UIMgr.Instance.ShowPanel<BeginPanel>("BeginPanel", E_UI_Layer.Bot);
                nowRoleObj.DestroyMe(null);
                break;
            case "BtnChange":
                //������ڻ��ˣ���ť��ִ���߼�
                if (isChanging)
                    return;
                //ά������
                //����������һ���ͱ�Ϊ��һ��
                nowRoleId = nowRoleId + 1 > DataMgr.Instance.roleInfoList.Count - 1 ? 0 : nowRoleId + 1;
                //�л�����
                ChangeRole(nowRoleId);
                break;
            case "BtnOk":
                //��ʾ�浵���
                UIMgr.Instance.ShowPanel<SaveLoadPanel>("SaveLoadPanel", E_UI_Layer.Bot, (panel) =>
                {
                    //�ı�SaveLoadPanel�ĵ�ǰ��ɫ�����ڴ����´浵
                    panel.roleId = nowRoleId;
                    panel.slType = E_SaveOrLoad.Save;
                });
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �л������߼������Ƴ���������ĳ���������
    /// </summary>
    /// <param name="id"></param>
    private void ChangeRole(int id)
    {
        //���ڻ���
        isChanging = true;

        //����������н�ɫ
        if (nowRoleObj != null)
        {
            //�����ٵ�ǰ��ɫ
            nowRoleObj.DestroyMe(() =>
            {
                //���ٽ����ٴ����µĽ�ɫ
                //��ý�ɫ����
                RoleInfo info = DataMgr.Instance.roleInfoList[id];
                //��BornPos����������
                ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
                {
                    //���ó�����
                    obj.transform.SetParent(bornPos, false);
                    //�滻��ǰ��ɫ
                    nowRoleObj = obj.AddComponent<RoleObj>();
                    nowRoleObj.DynamicMode();
                    //��������
                    isChanging = false;
                });
            });
        }
        //���������û�н�ɫ
        else
        {
            //ֱ�Ӵ����½�ɫ
            //��ý�ɫ����
            RoleInfo info = DataMgr.Instance.roleInfoList[id];
            //��BornPos����������
            ResMgr.Instance.LoadAsync<GameObject>(info.res, (obj) =>
            {
                //���ó�����
                obj.transform.SetParent(bornPos, false);
                //�滻��ǰ��ɫ
                nowRoleObj = obj.AddComponent<RoleObj>();
                nowRoleObj.DynamicMode();

                //��������
                isChanging = false;
            });
        }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //һ��ʼ�ҵ������㣬��ʼ����ɫ
        bornPos = GameObject.Find("BornPos").transform;
        ChangeRole(nowRoleId);

        //��ӽ�����Ϸʱ���¼�����
        customEvent = () => { UIMgr.Instance.HidePanel("ChooseRolePanel"); };
        EventCenter.Instance.AddListener("EnterGame", customEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        //�Ƴ�������Ϸʱ���¼�����
        EventCenter.Instance.RemoveListener("EnterGame", customEvent);
    }
}
