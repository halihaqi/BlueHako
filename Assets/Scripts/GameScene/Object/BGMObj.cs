using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMObj : SinletonAutoMono<BGMObj>
{
    void Start()
    {
        EventCenter.Instance.AddListener("LoadComplete", () =>
        {
            //���ű�������
            AudioMgr.Instance.PlayBkMusic("Audio/BGM_" + SceneManager.GetActiveScene().name);
        });

    }

    public void PlayBGM()
    {
        AudioMgr.Instance.PlayBkMusic("Audio/BGM_" + SceneManager.GetActiveScene().name);
    }

}
