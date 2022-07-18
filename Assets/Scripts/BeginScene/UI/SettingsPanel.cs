using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    //点击关闭
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        if(btnName == "BtnBack")
            UIMgr.Instance.HidePanel("SettingsPanel");
    }

    //改变声音开关
    //先改变声音，再改变数据
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

    //改变声音大小
    //先改变声音，再改变数据
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

    //显示时读取音乐数据,改变面板
    public override void ShowMe()
    {
        base.ShowMe();
        //改变控件值
        GetControl<Toggle>("TogMusic").isOn = DataMgr.Instance.audioData.musicOn;
        GetControl<Toggle>("TogSound").isOn = DataMgr.Instance.audioData.soundOn;
        GetControl<Slider>("SliderMusic").value = DataMgr.Instance.audioData.musicVolume;
        GetControl<Slider>("SliderSound").value = DataMgr.Instance.audioData.soundVolume;
    }

    //隐藏时保存音乐数据
    public override void HideMe()
    {
        base.HideMe();
        DataMgr.Instance.SaveAudioData();
    }
}
