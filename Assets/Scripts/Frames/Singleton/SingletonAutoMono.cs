using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式(继承Mono)
/// 自动创建空物体，不需要拖到场景中,过场景不销毁
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SinletonAutoMono<T> : MonoBehaviour where T : SinletonAutoMono<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

}
