using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    //����ر�
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if(btnName == "BtnBack")
            UIMgr.Instance.HidePanel("SettingsPanel");
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
