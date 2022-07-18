using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BasePanel : MonoBehaviour
{
    //UI组件容器，物体名对应物体挂载的所有UI组件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    //自定义触发事件，用于EventCenter.Instance.RemoveListener
    //通过命名事件来删除
    protected UnityAction customEvent;

    protected virtual void Awake()
    {
        //一开始搜索常用UI组件添加到容器中
        FindChildrenControls<Button>();
        FindChildrenControls<Image>();
        FindChildrenControls<Text>();
        FindChildrenControls<Toggle>();
        FindChildrenControls<Slider>();
    }

    /// <summary>
    /// 搜索所有子物体的特定UI组件并添加进字典容器中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControls<T>() where T : UIBehaviour
    {
        //搜索所有子物体的组件
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            //组件的物体名
            string objName = controls[i].gameObject.name;
            //如果字典包含，就添加到物体的组件List中
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);
            //如果不包含，就新建一个物体的组件List并添加此组件
            else
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            
            //添加事件监听
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
    /// 获得物体挂载的UI组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">物体名</param>
    /// <returns></returns>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        //每个物体只会挂载一个同种类的组件，所以不会重复
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

    #region 子类重写方法
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
    /// 面板渐变显示隐藏协程
    /// </summary>
    /// <param name="isIn">是否显示</param>
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