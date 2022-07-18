using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������࣬��¼�������ﱳ��
/// </summary>
public class BagData
{
    /// <summary>
    /// string������id��ͬ��BagInfoΪ��ǰ����ı���������
    /// </summary>
    public Dictionary<string, BagInfo> bagDic = new Dictionary<string, BagInfo>();
}

/// <summary>
/// �����������ݽṹ
/// </summary>
public class ItemData
{
    public ItemInfo info = null;
    public int num = 0;
}

/// <summary>
/// ��ǰ����ı���������
/// </summary>
public class BagInfo
{
    /// <summary>
    /// stringΪ������id,ItemDataΪÿ���Ӧ�ĵ�����Ϣ�͵�����
    /// </summary>
    public Dictionary<string, ItemData> slot;
    /// <summary>
    /// ��������
    /// </summary>
    public int bagCapacity = 16;

    /// <summary>
    /// ���ݱ������������ʼ���ձ���
    /// </summary>
    public BagInfo()
    {
        slot = new Dictionary<string, ItemData>(bagCapacity);
        for (int i = 0; i < bagCapacity; i++)
        {
            slot.Add(i.ToString(), new ItemData());
        }
    }
}
