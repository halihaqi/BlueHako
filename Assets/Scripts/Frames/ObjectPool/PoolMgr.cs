using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����ع�����
/// </summary>
public class PoolMgr : Singleton<PoolMgr>
{
    //���������
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject poolObj;

    #region ����ع�������
    /// <summary>
    /// �ӻ����ȡ��
    /// </summary>
    /// <param name="path">����·��</param>
    /// <param name="callback">ȡ��ص�</param>
    public void PopObj(string path, UnityAction<GameObject> callback)
    {
        //���������ز��ҳ�����δʹ�õ�����
        if (poolDic.ContainsKey(path) && poolDic[path].poolList.Count > 0)
        {
            GameObject obj = poolDic[path].Pop();
            callback?.Invoke(obj);
        }
        else
        {
            ResMgr.Instance.LoadAsync<GameObject>(path, (obj) =>
            {
                obj.name = path;
                callback?.Invoke(obj);
            });
        }
    }


    /// <summary>
    /// ѹ�뻺���
    /// </summary>
    /// <param name="path">����·��</param>
    /// <param name="obj">����ʵ��</param>
    public void PushObj(string path, GameObject obj)
    {
        //�������������
        if (poolDic.ContainsKey(path))
        {
            poolDic[path].Push(obj);
        }
        else
        {
            //���û�о��½�һ�������
            if (poolObj == null)
                poolObj = new GameObject("Pool");
            //new PoolData�����вι��캯����ִ�й���:
            //���ö���ظ�����
            //�½������
            //��objѹ������
            poolDic.Add(path, new PoolData(obj, poolObj));
        }
    }


    /// <summary>
    /// ��ն����
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
    #endregion




    #region ��������ݽṹ��(�ڲ���)
    private class PoolData
    {
        public GameObject parentObj;    //�Ӷ����
        public Stack<GameObject> poolList;  //����ض����б�

        /// <summary>
        /// ����ع��캯��
        /// </summary>
        /// <param name="obj">��һ������</param>
        /// <param name="poolObj">�����</param>
        public PoolData(GameObject obj, GameObject poolObj)
        {
            this.parentObj = new GameObject(obj.name);
            parentObj.transform.parent = poolObj.transform;
            poolList = new Stack<GameObject>();
            Push(obj);
        }

        /// <summary>
        /// �Ӷ����ȡ������
        /// </summary>
        /// <returns>�ó��Ķ���</returns>
        public GameObject Pop()
        {
            GameObject obj = poolList.Pop();
            obj.SetActive(true);
            obj.transform.SetParent(null, false);
            return obj;
        }

        /// <summary>
        /// ������ѹ�����
        /// </summary>
        /// <param name="obj">Ҫѹ��Ķ���</param>
        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(parentObj.transform, false);
            poolList.Push(obj);
        }

        /// <summary>
        /// ��ն����
        /// </summary>
        public void Clear()
        {
            parentObj = null;
            poolList.Clear();
        }
    }
    #endregion

}
