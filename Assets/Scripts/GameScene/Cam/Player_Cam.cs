using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    [Header("Base")]
    public Transform followTarget;//相机跟随目标
    public float sensitive = 12;//相机跟随敏感度
    public Vector3 shoulderOffset = Vector3.right;//跟随目标肩部偏移
    public float camDistance = 3;//相机距目标距离
    [Range(0f, 1f)]
    public float camSide = 0.6f;//画面相对于肩部偏移的左右偏移

    [Header("Obstacles")]
    public LayerMask camCollisionFilter = 1;//相机检测墙壁层级
    [Tag]
    public string ignoreTag = "Player";//检测忽略目标Tag，一般为目标物体Tag
    [Range(0.001f, 1f)]
    public float camRadius = 0.15f;//相机碰撞大小

    //射线检测
    private Ray ray;
    private RaycastHit hit;

    //目标位置
    private Vector3 offset;
    private Vector3 targetFollowPos;
    private Vector3 targetLookPos;
    private bool isTransmit = false;

    private void LateUpdate()
    {
        if (followTarget == null)
            return;

        //计算目标位置和旋转
        CalTargetPos();

        //如果被阻挡
        if (IsObstructed())
        {
            float dis = Vector3.Distance(hit.point, followTarget.position) - 0.2f;
            targetFollowPos = followTarget.position + offset - followTarget.forward * dis;
            if (!isTransmit)
            {
                transform.position = targetFollowPos;
                isTransmit = true;
            }
        }
        else
            isTransmit = false;

        //移动和旋转相机
        transform.position = Vector3.Lerp(transform.position, targetFollowPos, sensitive * Time.deltaTime);
        //transform.LookAt(targetLookPos);
        transform.rotation = Quaternion.LookRotation(targetLookPos - transform.position, Vector3.up);
        
    }

    /// <summary>
    /// 检测是否被阻挡
    /// </summary>
    /// <param name="collider">如果被阻挡，out被阻挡的物体</param>
    /// <returns></returns>
    private bool IsObstructed()
    {
        ray.origin = targetLookPos;
        ray.direction = transform.position - ray.origin;

        //Physics.SphereCast(ray, camRadius, out hit, 1000, camCollisionFilter);
        Physics.Raycast(ray, out hit, 1000, camCollisionFilter);
        if (hit.collider != null && hit.collider.tag != ignoreTag)
        {
            //如果玩家和障碍的距离短于玩家和相机的距离
            //说明视线被遮挡
            float dis = Vector3.Distance(followTarget.position, hit.point) - 0.5f;
            if(dis < Vector3.Distance(followTarget.position, transform.position))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 计算目标移动旋转
    /// </summary>
    private void CalTargetPos()
    {
        //计算相机位置和朝向偏移值
        offset = followTarget.right * shoulderOffset.x * (camSide - 0.5f) * 2
                    + followTarget.up * shoulderOffset.y
                        + followTarget.forward * shoulderOffset.z;
        targetFollowPos = followTarget.position + offset - followTarget.forward * camDistance;
        targetLookPos = followTarget.position + offset;
    }

    private void OnDrawGizmosSelected()
    {
        offset = followTarget.right * shoulderOffset.x * (camSide - 0.5f) * 2
                    + followTarget.up * shoulderOffset.y
                        + followTarget.forward * shoulderOffset.z;
        Vector3 targetFollowPos = followTarget.position + offset - followTarget.forward * camDistance;
        Vector3 targetLookPos = followTarget.position + offset;

        //相机位置
        Gizmos.color = Color.red;
        Gizmos.DrawLine(targetLookPos, targetFollowPos);
        Gizmos.DrawSphere(targetFollowPos, 0.05f);

        //观察位置
        Gizmos.color = Color.green;
        Gizmos.DrawLine(targetLookPos, targetLookPos);
        Gizmos.DrawSphere(targetLookPos, 0.05f);

        //遮挡检测范围
        Gizmos.color = Color.blue;
        //圆形检测
        ray.origin = targetLookPos;
        ray.direction = transform.position - targetLookPos;
        if (Physics.Raycast(ray, out hit, 1000, camCollisionFilter))
        {
            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawSphere(hit.point, camRadius);
        }
    }

}
