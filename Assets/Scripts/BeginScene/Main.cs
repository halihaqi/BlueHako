using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        //先读取音乐数据
        AudioData audioData = DataMgr.Instance.audioData;
        //再通过声音管理器改变音乐数据
        AudioMgr.Instance.ChangeBkMusicOn(audioData.musicOn);
        AudioMgr.Instance.ChangeBkMusicVolume(audioData.musicVolume);
        //再播放音乐
        BGMObj.Instance.PlayBGM();
        //显示开始面板
        UIMgr.Instance.ShowPanel<BeginPanel>("BeginPanel", E_UI_Layer.Bot);

        EventCenter.Instance.AddListener("EnterGame", () =>
        {
            UIMgr.Instance.ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.System);
        });
    }

}
