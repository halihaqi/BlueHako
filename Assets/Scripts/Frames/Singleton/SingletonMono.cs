using UnityEngine;

/// <summary>
/// ����ģʽ(�̳�Mono)
/// ������ɾ��
/// </summary>
/// <typeparam name="T"></typeparam>
[DisallowMultipleComponent]//ͬһ����ֻ�����һ������ű�
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
