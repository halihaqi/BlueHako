using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformTools
{
    /// <summary>
    /// �����Զ���Ƕȵķ�Χ��ŷ����
    /// (����������localEulerAngles,��Ϊ�����Զ��޸�)
    /// </summary>
    /// <param name="ifAngel"></param>
    /// <param name="ifMin"></param>
    /// <param name="ifMax"></param>
    /// <returns></returns>
    public static float ClampAngle(float ifAngel, float ifMin, float ifMax)
    {
        if (ifAngel < -360f)
            ifAngel += 360f;
        if (ifAngel > 360f)
            ifAngel -= 360f;
        return Mathf.Clamp(ifAngel, ifMin, ifMax);
    }
}
