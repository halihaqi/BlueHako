using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioMgr : Singleton<AudioMgr>
{
    //��������
    private AudioSource bkMusic = null;
    private float bkMusicVolume = 1;
    private bool bkMusicOn = true;

    //��Ч
    private GameObject soundObj = null;
    private Dictionary<string, AudioSource> soundDic = new Dictionary<string, AudioSource>();
    private float soundVolume = 1;
    private bool soundOn = true;

    public AudioMgr()
    {
        MonoMgr.Instance.AddUpdateListener(MusicUpdate);
    }

    /// <summary>
    /// ÿ֡�ж���Ч�Ƿ񲥷����
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


    #region �������ֹ�������
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="path">����·��</param>
    public void PlayBkMusic(string path)
    {
        //���Ϊ�գ��ڳ������½������AudioSource
        if (bkMusic == null)
        {
            GameObject obj = new GameObject("BkMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //�첽������Ч
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
    /// �ı䱳�����ִ�С
    /// </summary>
    /// <param name="Volume">����</param>
    public void ChangeBkMusicVolume(float Volume)
    {
        bkMusicVolume = Volume;
        if (bkMusic != null)
            bkMusic.volume = bkMusicVolume;
    }

    /// <summary>
    /// �ı䱳�����ֿ���
    /// </summary>
    /// <param name="isOn">�Ƿ���</param>
    public void ChangeBkMusicOn(bool isOn)
    {
        bkMusicOn = isOn;
        if(bkMusic != null)
            bkMusic.mute = !bkMusicOn;
    }

    /// <summary>
    /// ��ͣ��������
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
    /// ֹͣ��������
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

    #region ��Ч��������
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="path">��Ч·��</param>
    /// <param name="callback">�ص�����,���source</param>
    public void PlaySound(string path, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        //���������û�о��½�Sound������
        if(soundObj == null)
            soundObj = new GameObject("Sound");
        //�첽������Ч��������ɺ󲥷Ų�����soundList
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
    /// �ı䵥����Ч��С
    /// </summary>
    /// <param name="path">��Ч·��</param>
    /// <param name="volume">����</param>
    public void ChangeSoundVolume(string path, float volume)
    {
        if (soundDic.ContainsKey(path))
            soundDic[path].volume = volume;
    }

    /// <summary>
    /// �ı�������Ч��С
    /// </summary>
    /// <param name="volume">����</param>
    public void ChangeAllSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach (string key in soundDic.Keys)
        {
            soundDic[key].volume = volume;
        }
    }

    /// <summary>
    /// �ı䵥����Ч����
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="isOn">�Ƿ���</param>
    public void ChangeSoundOn(string path, bool isOn)
    {
        if (soundDic.ContainsKey(path))
            soundDic[path].mute = !isOn;
    }

    /// <summary>
    /// �ı�������Ч����
    /// </summary>
    /// <param name="isOn">�Ƿ���</param>
    public void ChangeAllSoundOn(bool isOn)
    {
        soundOn = isOn;
        foreach (string key in soundDic.Keys)
        {
            soundDic[key].mute = !isOn;
        }
    }

    /// <summary>
    /// ֹͣ��Ч
    /// </summary>
    /// <param name="source">��Ч���</param>
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
    /// ֹͣ������Ч
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
