using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioMgr : Singleton<AudioMgr>
{
    //背景音乐
    private AudioSource bkMusic = null;
    private float bkMusicVolume = 1;
    private bool bkMusicOn = true;

    //音效
    private GameObject soundObj = null;
    private Dictionary<string, AudioSource> soundDic = new Dictionary<string, AudioSource>();
    private float soundVolume = 1;
    private bool soundOn = true;

    public AudioMgr()
    {
        MonoMgr.Instance.AddUpdateListener(MusicUpdate);
    }

    /// <summary>
    /// 每帧判断音效是否播放完毕
    /// </summary>
    private void MusicUpdate()
    {
        foreach (string key in soundDic.Keys)
        {
            if (!soundDic[key].isPlaying)
            {
                GameObject.Destroy(soundDic[key]);
                soundDic.Remove(key);
            }
        }
    }


    #region 背景音乐公共方法
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="path">音乐路径</param>
    public void PlayBkMusic(string path)
    {
        //如果为空，在场景中新建并添加AudioSource
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BkMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //异步加载音效
        ResMgr.Instance.LoadAsync<AudioClip>(path, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.mute = !bkMusicOn;
            bkMusic.volume = bkMusicVolume;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐大小
    /// </summary>
    /// <param name="Volume">音量</param>
    public void ChangeBkMusicVolume(float Volume)
    {
        bkMusicVolume = Volume;
        if (bkMusic != null)
            bkMusic.volume = bkMusicVolume;
    }

    /// <summary>
    /// 改变背景音乐开关
    /// </summary>
    /// <param name="isOn">是否开启</param>
    public void ChangeBkMusicOn(bool isOn)
    {
        bkMusicOn = isOn;
        if(bkMusic != null)
            bkMusic.mute = !bkMusicOn;
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBkMusic()
    {
        if (bkMusic == null)
        {
            Debug.Log("No BkMusic in Scene");
            return;
        }
        bkMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bkMusic == null)
        {
            Debug.Log("No BkMusic in Scene");
            return;
        }
        bkMusic.Stop();
    }
    #endregion

    #region 音效公共方法
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">音效路径</param>
    /// <param name="callback">回调函数,获得source</param>
    public void PlaySound(string path, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        //如果场景中没有就新建Sound空物体
        if(soundObj == null)
            soundObj = new GameObject("Sound");
        //异步加载音效，加载完成后播放并加入soundList
        ResMgr.Instance.LoadAsync<AudioClip>(path, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = soundVolume;
            source.mute = !soundOn;
            source.loop = isLoop;
            source.Play();
            soundDic.Add(path, source);
            callback?.Invoke(source);
        });
    }

    /// <summary>
    /// 改变单个音效大小
    /// </summary>
    /// <param name="path">音效路径</param>
    /// <param name="volume">音量</param>
    public void ChangeSoundVolume(string path, float volume)
    {
        if (soundDic.ContainsKey(path))
            soundDic[path].volume = volume;
    }

    /// <summary>
    /// 改变所有音效大小
    /// </summary>
    /// <param name="volume">音量</param>
    public void ChangeAllSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach (string key in soundDic.Keys)
        {
            soundDic[key].volume = volume;
        }
    }

    /// <summary>
    /// 改变单个音效开关
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="isOn">是否开启</param>
    public void ChangeSoundOn(string path, bool isOn)
    {
        if (soundDic.ContainsKey(path))
            soundDic[path].mute = !isOn;
    }

    /// <summary>
    /// 改变所有音效开关
    /// </summary>
    /// <param name="isOn">是否开启</param>
    public void ChangeAllSoundOn(bool isOn)
    {
        soundOn = isOn;
        foreach (string key in soundDic.Keys)
        {
            soundDic[key].mute = !isOn;
        }
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="source">音效组件</param>
    public void StopSound(string path)
    {
        if (!soundDic.ContainsKey(path))
        {
            Debug.Log("No Sound in Scene");
            return;
        }
        soundDic[path].Stop();
        Object.Destroy(soundDic[path]);
        soundDic.Remove(path);
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllSound()
    {
        foreach (string key in soundDic.Keys)
        {
            soundDic[key].Stop();
            Object.Destroy(soundDic[key]);
        }
        soundDic.Clear();
    }

    #endregion

}
