using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 开始场景角色种类
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
        //找到出生点和展示点
        bornPos = GameObject.Find("BornPos").transform;
        showPos = GameObject.Find("ShowPos").transform;
        GetComponent<CharacterController>().enabled = false;
        //设置Choose Layer权重为1
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);
        //开启展示逻辑
        ShowMe();
    }

    private void Update()
    {
        if (roleType == E_RoleType.Static)
            return;
        if (!canPick)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //如果按下鼠标并且射线检测到角色碰撞体
        //就可以拖拽
        if (Physics.Raycast(ray,1000,1<<LayerMask.NameToLayer("Role")) && Input.GetMouseButtonDown(0))
        {
            isDruging = true;
        }

        //如果可以拖拽
        if (isDruging)
        {
            //如果一直按下，执行拖拽逻辑
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("PickUp", true);
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(Camera.main.transform.position.x - transform.position.x);
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
                targetPos.x = transform.position.x;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * pickSpeed);
            }
            //如果松开鼠标，解除拖拽可以拖拽状态
            else if (Input.GetMouseButtonUp(0))
            {
                transform.position = showPos.position;
                anim.SetBool("PickUp", false);
                isDruging = false;
            }
        }
    }

    #region 转身和移动协程
    /// <summary>
    /// 转身协程
    /// </summary>
    /// <param name="targetRot">转身朝向</param>
    /// <param name="callback">转身结束的回调</param>
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
    /// 移动协程
    /// </summary>
    /// <param name="targetPos">目标点</param>
    /// <param name="callback">到达目标的回调</param>
    /// <returns></returns>
    IEnumerator Move(Vector3 targetPos, UnityAction callback)
    {
        while (transform.position != targetPos)
        {
            //匀速移动到目标点
            transform.position = Vector3.MoveTowards
                (transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        callback?.Invoke();
    }
    #endregion

    /// <summary>
    /// 出生后移动到展示点逻辑
    /// </summary>
    private void ShowMe()
    {
        //设置行走动画
        anim.SetBool("Walk", true);
        //移动到展示点
        StartCoroutine(Move(showPos.position, () =>
        {
            //移动结束后
            //切换到idle状态
            anim.SetBool("Walk", false);
            //先转过身来
            StartCoroutine(TurnLeft(Vector3.right, () =>
            {
                //先切换姿势动画，再返回到站立动画
                anim.SetTrigger("Reaction");
                canPick = true;
            }));
        }));
    }

    #region 外部调用方法

    /// <summary>
    /// 返回后销毁自己逻辑
    /// </summary>
    /// <param name="callback">销毁前的回调</param>
    public void DestroyMe(UnityAction callback)
    {
        roleType = E_RoleType.Static;
        //终止所有协程,防止上一个协程还在执行
        StopAllCoroutines();
        //禁止拖拽
        canPick = false;
        anim.SetBool("PickUp", false);


        //先转身
        StartCoroutine(TurnLeft(Vector3.forward, () =>
        {
            //转身结束后
            //设置行走动画
            anim.SetBool("Walk", true);
            //移动到出生点
            StartCoroutine(Move(bornPos.position, () =>
            {
                //移动结束后销毁
                callback?.Invoke();
                Destroy(gameObject);
            }));
        }));
    }

    /// <summary>
    /// 切换为选角的逻辑
    /// </summary>
    public void DynamicMode()
    {
        roleType = E_RoleType.Dynamic;
    }

    /// <summary>
    /// 切换为静止逻辑
    /// </summary>
    public void StaticMode()
    {
        roleType = E_RoleType.Static;
    }
    #endregion

}
