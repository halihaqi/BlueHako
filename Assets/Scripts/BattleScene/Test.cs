using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        DataMgr.Instance.LoadPlayerInfo(0);
        GameMgr.Instance.InitPlayer();
    }
}
