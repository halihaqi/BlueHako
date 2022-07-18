using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    //UI�����������������Ӧ������ص�����UI���
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    //�Զ��崥���¼�������EventCenter.Instance.RemoveListener
    //ͨ�������¼���ɾ��
    protected UnityAction customEvent;

    protected virtual void Awake()
    {
        //һ��ʼ��������UI�����ӵ�������
        FindChildrenControls<Button>();
        FindChildrenControls<Image>();
        FindChildrenControls<Text>();
        FindChildrenControls<Toggle>();
        FindChildrenControls<Slider>();
    }

    /// <summary>
    /// ����������������ض�UI�������ӽ��ֵ�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControls<T>() where T : UIBehaviour
    {
        //������������������
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            //�����������
            string objName = controls[i].gameObject.name;
            //����ֵ����������ӵ���������List��
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);
            //��������������½�һ����������List����Ӵ����
            else
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            
            //����¼�����
            if(controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((isToggle) =>
                {
                    ToggleOnValueChanged(objName, isToggle);
                });
            }
            if (controls[i] is Slider)
            {
                (controls[i] as Slider).onValueChanged.AddListener((val) =>
                {
                    SliderOnValueChanged(objName, val);
                });
            }
            if (controls[i] is InputField)
            {
                (controls[i] as InputField).onValueChanged.AddListener((val) =>
                {
                    InputFieldOnValueChanged(objName, val);
                });
            }
        }
    }

    /// <summary>
    /// ���������ص�UI���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="name">������</param>
    /// <returns></returns>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        //ÿ������ֻ�����һ��ͬ�������������Բ����ظ�
        if (controlDic.ContainsKey(name))
        {
            for (int i = 0; i < controlDic[name].Count; i++)
            {
                if (controlDic[name][i] is T)
                    return controlDic[name][i] as T;
            }
        }
        print(name + " No this UIControl");
        return null;
    }

    #region ������д����
    public virtual void ShowMe()
    {
        StartCoroutine(FadeInOut(true));
    }

    public virtual void HideMe()
    {
        StartCoroutine(FadeInOut(false));
    }

    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void ToggleOnValueChanged(string togName, bool isToggle)
    {

    }

    protected virtual void SliderOnValueChanged(string sldName, float val)
    {

    }

    protected virtual void InputFieldOnValueChanged(string inputName, string val)
    {

    }
    #endregion

    /// <summary>
    /// ��彥����ʾ����Э��
    /// </summary>
    /// <param name="isIn">�Ƿ���ʾ</param>
    /// <returns></returns>
    IEnumerator FadeInOut(bool isIn)
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        if (isIn)
        {
            canvasGroup.alpha = 0;
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += 0.1f;
                yield return null;
            }
        }
        else
        {
            canvasGroup.alpha = 1;
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 0.1f;
                yield return null;
            }
        }
    }
}