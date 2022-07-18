using UnityEngine;

/// <summary>
/// 单例模式(继承Mono)
/// 过场景删除
/// </summary>
/// <typeparam name="T"></typeparam>
[DisallowMultipleComponent]//同一物体只允许挂一次这个脚本
public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
