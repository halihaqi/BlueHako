using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ(�̳�Mono)
/// �Զ����������壬����Ҫ�ϵ�������,������������
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
