using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 1;
    void Start()
    {
        Invoke("DestroyMe", destroyTime);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
