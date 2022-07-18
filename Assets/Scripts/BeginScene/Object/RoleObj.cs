using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʼ������ɫ����
/// </summary>
public enum E_RoleType
{
    Static, Dynamic
}


public class RoleObj : MonoBehaviour
{
    private E_RoleType roleType = E_RoleType.Static;
    public float moveSpeed = 1.5f;
    public float rotateSpeed = 10;
    public float pickSpeed = 3;

    private Transform bornPos;
    private Transform showPos;
    private Animator anim;

    private bool canPick = false;
    private bool isDruging = false;

    private void Start()
    {
        if (roleType == E_RoleType.Static)
            return;
        //�ҵ��������չʾ��
        bornPos = GameObject.Find("BornPos").transform;
        showPos = GameObject.Find("ShowPos").transform;
        GetComponent<CharacterController>().enabled = false;
        //����Choose LayerȨ��Ϊ1
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);
        //����չʾ�߼�
        ShowMe();
    }

    private void Update()
    {
        if (roleType == E_RoleType.Static)
            return;
        if (!canPick)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //���������겢�����߼�⵽��ɫ��ײ��
        //�Ϳ�����ק
        if (Physics.Raycast(ray,1000,1<<LayerMask.NameToLayer("Role")) && Input.GetMouseButtonDown(0))
        {
            isDruging = true;
        }

        //���������ק
        if (isDruging)
        {
            //���һֱ���£�ִ����ק�߼�
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("PickUp", true);
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(Camera.main.transform.position.x - transform.position.x);
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
                targetPos.x = transform.position.x;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * pickSpeed);
            }
            //����ɿ���꣬�����ק������ק״̬
            else if (Input.GetMouseButtonUp(0))
            {
                transform.position = showPos.position;
                anim.SetBool("PickUp", false);
                isDruging = false;
            }
        }
    }

    #region ת����ƶ�Э��
    /// <summary>
    /// ת��Э��
    /// </summary>
    /// <param name="targetRot">ת����</param>
    /// <param name="callback">ת������Ļص�</param>
    /// <returns></returns>
    IEnumerator TurnLeft(Vector3 targetDir, UnityAction callback)
    {
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        while (transform.rotation != targetRot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
            yield return null;
        }
        callback?.Invoke();
    }

    /// <summary>
    /// �ƶ�Э��
    /// </summary>
    /// <param name="targetPos">Ŀ���</param>
    /// <param name="callback">����Ŀ��Ļص�</param>
    /// <returns></returns>
    IEnumerator Move(Vector3 targetPos, UnityAction callback)
    {
        while (transform.position != targetPos)
        {
            //�����ƶ���Ŀ���
            transform.position = Vector3.MoveTowards
                (transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        callback?.Invoke();
    }
    #endregion

    /// <summary>
    /// �������ƶ���չʾ���߼�
    /// </summary>
    private void ShowMe()
    {
        //�������߶���
        anim.SetBool("Walk", true);
        //�ƶ���չʾ��
        StartCoroutine(Move(showPos.position, () =>
        {
            //�ƶ�������
            //�л���idle״̬
            anim.SetBool("Walk", false);
            //��ת������
            StartCoroutine(TurnLeft(Vector3.right, () =>
            {
                //���л����ƶ������ٷ��ص�վ������
                anim.SetTrigger("Reaction");
                canPick = true;
            }));
        }));
    }

    #region �ⲿ���÷���

    /// <summary>
    /// ���غ������Լ��߼�
    /// </summary>
    /// <param name="callback">����ǰ�Ļص�</param>
    public void DestroyMe(UnityAction callback)
    {
        roleType = E_RoleType.Static;
        //��ֹ����Э��,��ֹ��һ��Э�̻���ִ��
        StopAllCoroutines();
        //��ֹ��ק
        canPick = false;
        anim.SetBool("PickUp", false);


        //��ת��
        StartCoroutine(TurnLeft(Vector3.forward, () =>
        {
            //ת�������
            //�������߶���
            anim.SetBool("Walk", true);
            //�ƶ���������
            StartCoroutine(Move(bornPos.position, () =>
            {
                //�ƶ�����������
                callback?.Invoke();
                Destroy(gameObject);
            }));
        }));
    }

    /// <summary>
    /// �л�Ϊѡ�ǵ��߼�
    /// </summary>
    public void DynamicMode()
    {
        roleType = E_RoleType.Dynamic;
    }

    /// <summary>
    /// �л�Ϊ��ֹ�߼�
    /// </summary>
    public void StaticMode()
    {
        roleType = E_RoleType.Static;
    }
    #endregion

}
