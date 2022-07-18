using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFollow : MonoBehaviour
{
    public float moveSpeed = 10;

    private Transform targetPos;
    private bool isSetTarget = false;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    void Update()
    {
        if (isSetTarget)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPos.position) < 0.1f || targetPos == null)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置发射目标
    /// </summary>
    /// <param name="targetPos"></param>
    public void SetTarget(Transform firePos, Transform targetPos)
    {
        transform.position = firePos.position;
        this.targetPos = targetPos;
        isSetTarget = true;
    }
}
