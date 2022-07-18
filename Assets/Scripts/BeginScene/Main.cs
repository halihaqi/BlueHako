using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        //�ȶ�ȡ��������
        AudioData audioData = DataMgr.Instance.audioData;
        //��ͨ�������������ı���������
        AudioMgr.Instance.ChangeBkMusicOn(audioData.musicOn);
        AudioMgr.Instance.ChangeBkMusicVolume(audioData.musicVolume);
        //�ٲ�������
        BGMObj.Instance.PlayBGM();
        //��ʾ��ʼ���
        UIMgr.Instance.ShowPanel<BeginPanel>("BeginPanel", E_UI_Layer.Bot);

        EventCenter.Instance.AddListener("EnterGame", () =>
        {
            UIMgr.Instance.ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.System);
        });
    }

}
