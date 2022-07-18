using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class int_Expand
{
    /// <summary>
    /// ��intת��Ϊtime��ʽ�ַ��� 00:00:00
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string ToTime(this int i)
    {
        if (i < 0)
            return "0";
        int s = i % 60;
        i = i / 60;
        int m = i % 60;
        i = i / 60;
        int h = i;
        return h + ":" + m + ":" + s;
    }
}
