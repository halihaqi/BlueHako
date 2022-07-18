using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : Singleton<InputMgr>
{
    //�Ƿ���������
    bool isOpenCheck = false;
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(InputUpdate);
    }

    /// <summary>
    /// ���Key����
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
    /// ��GlobalMonoÿ֡����
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


    #region �ⲿ���÷���
    /// <summary>
    /// �����ر�������
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrClose(bool isOpen)
    {
        isOpenCheck = isOpen;
    }

    #endregion

}
