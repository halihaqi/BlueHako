using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : Singleton<InputMgr>
{
    //是否开启输入检测
    bool isOpenCheck = false;
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(InputUpdate);
    }

    /// <summary>
    /// 检查Key输入
    /// </summary>
    /// <param name="key"></param>
    private void KeyCheck(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            EventCenter.Instance.PostEvent("GetKeyDown", key);
        if(Input.GetKeyUp(key))
            EventCenter.Instance.PostEvent("GetKeyUp", key);
        if(Input.GetKey(key))
            EventCenter.Instance.PostEvent("GetKey", key);
    }

    /// <summary>
    /// 在GlobalMono每帧调用
    /// </summary>
    private void InputUpdate()
    {
        if (!isOpenCheck)
            return;
        KeyCheck(KeyCode.B);
        KeyCheck(KeyCode.Q);
        KeyCheck(KeyCode.E);
        KeyCheck(KeyCode.Tab);
        KeyCheck(KeyCode.Escape);
    }


    #region 外部调用方法
    /// <summary>
    /// 开启关闭输入检测
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrClose(bool isOpen)
    {
        isOpenCheck = isOpen;
    }

    #endregion

}
