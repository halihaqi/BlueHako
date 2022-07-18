using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器
/// </summary>
public class PoolMgr : Singleton<PoolMgr>
{
    //缓存池容器
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject poolObj;

    #region 对象池公共方法
    /// <summary>
    /// 从缓存池取出
    /// </summary>
    /// <param name="path">物体路径</param>
    /// <param name="callback">取完回调</param>
    public void PopObj(string path, UnityAction<GameObject> callback)
    {
        //如果有这个池并且池里有未使用的物体
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
    /// 压入缓存池
    /// </summary>
    /// <param name="path">物体路径</param>
    /// <param name="obj">物体实例</param>
    public void PushObj(string path, GameObject obj)
    {
        //如果有这个缓存池
        if (poolDic.ContainsKey(path))
        {
            poolDic[path].Push(obj);
        }
        else
        {
            //如果没有就新建一个缓存池
            if (poolObj == null)
                poolObj = new GameObject("Pool");
            //new PoolData调用有参构造函数，执行过程:
            //设置对象池父对象
            //新建对象池
            //将obj压入对象池
            poolDic.Add(path, new PoolData(obj, poolObj));
        }
    }


    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
    #endregion




    #region 对象池数据结构类(内部类)
    private class PoolData
    {
        public GameObject parentObj;    //子对象池
        public Stack<GameObject> poolList;  //对象池对象列表

        /// <summary>
        /// 对象池构造函数
        /// </summary>
        /// <param name="obj">第一个对象</param>
        /// <param name="poolObj">对象池</param>
        public PoolData(GameObject obj, GameObject poolObj)
        {
            this.parentObj = new GameObject(obj.name);
            parentObj.transform.parent = poolObj.transform;
            poolList = new Stack<GameObject>();
            Push(obj);
        }

        /// <summary>
        /// 从对象池取出对象
        /// </summary>
        /// <returns>拿出的对象</returns>
        public GameObject Pop()
        {
            GameObject obj = poolList.Pop();
            obj.SetActive(true);
            obj.transform.SetParent(null, false);
            return obj;
        }

        /// <summary>
        /// 向对象池压入对象
        /// </summary>
        /// <param name="obj">要压入的对象</param>
        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(parentObj.transform, false);
            poolList.Push(obj);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            parentObj = null;
            poolList.Clear();
        }
    }
    #endregion

}
