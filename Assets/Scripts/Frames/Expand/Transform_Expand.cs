using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Transform_Expand
{
    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="root"></param>
    /// <param name="name">��������</param>
    /// <returns></returns>
    public static Transform DeepGetChild(this Transform root, string name)
    {
        if (root.name == name)
            return root;
        Transform target = null;
        for (int i = 0; i < root.childCount; i++)
        {
            target = root.GetChild(i).DeepGetChild(name);
            if (target != null)
                break;
        }
        return target;
    }
}
