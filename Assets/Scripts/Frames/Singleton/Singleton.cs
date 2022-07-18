using System;

/// <summary>
/// ����ģʽ����(���̳�Mono)
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}


#region Pro�棬�ж��߳�������˽�й��캯��
///// <summary>
///// ����ģʽ����(���̳�Mono)
///// �ж��߳��� ��˽�й��캯��
///// </summary>
///// <typeparam name="T"></typeparam>
//public abstract class Singleton<T> where T : Singleton<T>
//{
//    private static T instance;
//    private static readonly object obj = new object();
//    public static T Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                lock (obj)
//                {
//                    if (instance == null)
//                    {
//                        instance = Activator.CreateInstance<T>();
//                    }
//                }
//            }
//            return instance;
//        }
//    }
//}
#endregion
