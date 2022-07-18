using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    [Header("Base")]
    public Transform followTarget;//�������Ŀ��
    public float sensitive = 12;//����������ж�
    public Vector3 shoulderOffset = Vector3.right;//����Ŀ��粿ƫ��
    public float camDistance = 3;//�����Ŀ�����
    [Range(0f, 1f)]
    public float camSide = 0.6f;//��������ڼ粿ƫ�Ƶ�����ƫ��

    [Header("Obstacles")]
    public LayerMask camCollisionFilter = 1;//������ǽ�ڲ㼶
    [Tag]
    public string ignoreTag = "Player";//������Ŀ��Tag��һ��ΪĿ������Tag
    [Range(0.001f, 1f)]
    public float camRadius = 0.15f;//�����ײ��С

    //���߼��
    private Ray ray;
    private RaycastHit hit;

    //Ŀ��λ��
    private Vector3 offset;
    private Vector3 targetFollowPos;
    private Vector3 targetLookPos;
    private bool isTransmit = false;

    private void LateUpdate()
    {
        if (followTarget == null)
            return;

        //����Ŀ��λ�ú���ת
        CalTargetPos();

        //������赲
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

        //�ƶ�����ת���
        transform.position = Vector3.Lerp(transform.position, targetFollowPos, sensitive * Time.deltaTime);
        //transform.LookAt(targetLookPos);
        transform.rotation = Quaternion.LookRotation(targetLookPos - transform.position, Vector3.up);
        
    }

    /// <summary>
    /// ����Ƿ��赲
    /// </summary>
    /// <param name="collider">������赲��out���赲������</param>
    /// <returns></returns>
    private bool IsObstructed()
    {
        ray.origin = targetLookPos;
        ray.direction = transform.position - ray.origin;

        //Physics.SphereCast(ray, camRadius, out hit, 1000, camCollisionFilter);
        Physics.Raycast(ray, out hit, 1000, camCollisionFilter);
        if (hit.collider != null && hit.collider.tag != ignoreTag)
        {
            //�����Һ��ϰ��ľ��������Һ�����ľ���
            //˵�����߱��ڵ�
            float dis = Vector3.Distance(followTarget.position, hit.point) - 0.5f;
            if(dis < Vector3.Distance(followTarget.position, transform.position))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ����Ŀ���ƶ���ת
    /// </summary>
    private void CalTargetPos()
    {
        //�������λ�úͳ���ƫ��ֵ
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

        //���λ��
        Gizmos.color = Color.red;
        Gizmos.DrawLine(targetLookPos, targetFollowPos);
        Gizmos.DrawSphere(targetFollowPos, 0.05f);

        //�۲�λ��
        Gizmos.color = Color.green;
        Gizmos.DrawLine(targetLookPos, targetLookPos);
        Gizmos.DrawSphere(targetLookPos, 0.05f);

        //�ڵ���ⷶΧ
        Gizmos.color = Color.blue;
        //Բ�μ��
        ray.origin = targetLookPos;
        ray.direction = transform.position - targetLookPos;
        if (Physics.Raycast(ray, out hit, 1000, camCollisionFilter))
        {
            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawSphere(hit.point, camRadius);
        }
    }

}
