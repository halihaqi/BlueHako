using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : BasePanel
{
    public Button btnExit;
    public Text txtExit;
    public Text txtCancel;

    protected override void OnClick(string btnName)
    {
        if (btnName == "BtnCancel")
            UIMgr.Instance.HidePanel("ExitPanel");
        else
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("�Ƿ�ȷ�����沢�˳���", () =>
                {
                    DataMgr.Instance.NowPlayerInfo.time = (int)Time.time;
                    DataMgr.Instance.SavePlayerData();
                    UIMgr.Instance.HideAllPanel();
                    SceneMgr.Instance.LoadSceneAsyncPro("BeginScene", null);
                });
            });
    }

    /// <summary>
    /// ս�������˳����ģʽ
    /// </summary>
    public void BattleSceneMode()
    {
        txtCancel.text = "����ս��";
        txtExit.text = "�˳�ս��";
        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(() =>
        {
            UIMgr.Instance.ShowPanel<TipPanel>("TipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeTipInfo("�Ƿ��˳�ս�������ʧȥս��Ʒ", () =>
                {
                    UIMgr.Instance.HideAllPanel();
                    SceneMgr.Instance.LoadSceneAsyncPro("GameScene", () =>
                    {
                        GameMgr.Instance.InitPlayer();
                        //����GameUI
                        UIMgr.Instance.ShowPanel<GamePanel>("GamePanel", E_UI_Layer.Bot, (panel) =>
                        {
                            panel.Init(DataMgr.Instance.NowPlayerInfo);
                        });
                    });
                });
            });
        });
    }

    //�ı���������
    //�ȸı��������ٸı�����
    protected override void ToggleOnValueChanged(string togName, bool isToggle)
    {
        base.ToggleOnValueChanged(togName, isToggle);
        switch (togName)
        {
            case "TogMusic":
                AudioMgr.Instance.ChangeBkMusicOn(isToggle);
                DataMgr.Instance.audioData.musicOn = isToggle;
                break;
            case "TogSound":
                AudioMgr.Instance.ChangeAllSoundOn(isToggle);
                DataMgr.Instance.audioData.soundOn = isToggle;
                break;
            default:
                break;
        }
    }

    //�ı�������С
    //�ȸı��������ٸı�����
    protected override void SliderOnValueChanged(string sldName, float val)
    {
        base.SliderOnValueChanged(sldName, val);
        switch (sldName)
        {
            case "SliderMusic":
                AudioMgr.Instance.ChangeBkMusicVolume(val);
                DataMgr.Instance.audioData.musicVolume = val;
                break;
            case "SliderSound":
                AudioMgr.Instance.ChangeAllSoundVolume(val);
                DataMgr.Instance.audioData.soundVolume = val;
                break;
            default:
                break;
        }
    }

    //��ʾʱ��ȡ��������,�ı����
    public override void ShowMe()
    {
        base.ShowMe();
        //�ı�ؼ�ֵ
        GetControl<Toggle>("TogMusic").isOn = DataMgr.Instance.audioData.musicOn;
        GetControl<Toggle>("TogSound").isOn = DataMgr.Instance.audioData.soundOn;
        GetControl<Slider>("SliderMusic").value = DataMgr.Instance.audioData.musicVolume;
        GetControl<Slider>("SliderSound").value = DataMgr.Instance.audioData.soundVolume;
    }

    //����ʱ������������
    public override void HideMe()
    {
        base.HideMe();
        DataMgr.Instance.SaveAudioData();
    }
}
