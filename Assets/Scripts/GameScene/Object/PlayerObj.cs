using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerObj : MonoBehaviour
{
    [Header("Base")]
    public float moveSpeed = 3;
    public float sprintSpeed = 5;
    public float rotateSpeed = 50;
    public float rotateSmoothTime = 0.12f;

    [Header("Gravity")]
    public float gravity = -15;

    [Header("FollowCamera")]
    public float topClamp = 70;//相机最大仰角
    public float bottomClamp = -30;//相机最大俯角
    public bool isFilpPitch = true;

    [Header("GroundCheck")]
    public bool isGrounded = true;
    public float groundOffset = -0.29f;
    public float groundRadius = 0.28f;
    public LayerMask groundLayers = 1;

    //输入参数
    private Vector2 _inputLook;
    private Vector2 _inputMove;
    private float _threshold = 0.01f;//输入最低门槛

    //移动参数
    private float _targetRot = 0;
    private float _rotVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53;

    //相机参数
    private Camera followCamera;
    private Transform followTarget;
    private float _camTargetYaw;
    private float _camTargetPitch;

    //输入事件监听
    protected UnityAction<KeyCode> inputEvent;

    //Component
    protected Animator anim;
    protected CharacterController cc;

    protected virtual void Awake()
    {
        //获取组件
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //找到相机跟随点
        followTarget = transform.Find("FollowPos");
        followCamera = Camera.main;
        followCamera.GetComponent<Player_Cam>().followTarget = followTarget;
        //打开输入监听
        InputMgr.Instance.OpenOrClose(true);
        //按下B打开或关闭背包
        //按下Tab打开任务列表
        inputEvent = (key) =>
        {
            if (key == KeyCode.B)
            {
                BagMgr.Instance.OpenOrCloseBag();
            }
            if(key == KeyCode.Tab)
            {
                TaskMgr.OpenOrCloseTask();
            }
            if( key == KeyCode.Escape)
            {
                UIMgr.Instance.ShowPanel<ExitPanel>("ExitPanel", E_UI_Layer.Bot);
            }
        };
        EventCenter.Instance.AddListener<KeyCode>("GetKeyDown", inputEvent);
    }

    protected virtual void OnDestroy()
    {
        //关闭输入监听
        InputMgr.Instance.OpenOrClose(false);
        EventCenter.Instance.RemoveListener<KeyCode>("GetKeyDown", inputEvent);
    }

    protected virtual void Update()
    {
        Gravity();
        GroundCheck();
        UpdateInput();
        Move();
        CameraTargetRotation();       
    }

    /// <summary>
    /// 更新输入参数
    /// </summary>
    private void UpdateInput()
    {
        _inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// 移动和转身
    /// </summary>
    private void Move()
    {
        //判断是否为冲刺速度
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        //判断移动输入是否为0
        if (_inputMove == Vector2.zero)
            targetSpeed = 0;

        //输入的方向
        Vector3 inputDir = new Vector3(_inputMove.x, 0, _inputMove.y);

        //人物移动时的八向旋转
        if(_inputMove != Vector2.zero)
        {
            _targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + followCamera.transform.eulerAngles.y;
            float rot = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRot, ref _rotVelocity, rotateSmoothTime);

            transform.rotation = Quaternion.Euler(0, rot, 0);
        }

        //旋转后的移动方向
        Vector3 targetDir = Quaternion.Euler(0, _targetRot, 0) * Vector3.forward;

        //移动人物,加上垂直速度
        cc.Move((targetDir.normalized + Vector3.up * _verticalVelocity) * targetSpeed * Time.deltaTime);
        //移动动画
        anim.SetFloat("Speed", Input.GetKey(KeyCode.LeftShift) ? inputDir.magnitude : inputDir.magnitude / 2);
    }

    /// <summary>
    /// 地面检测
    /// </summary>
    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// 重力
    /// </summary>
    private void Gravity()
    {
        if (!isGrounded)
        {
            //如果没有达到垂直速度阈值，就会一直加垂直速度
            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = 0;
        }
    }

    /// <summary>
    /// 第三人称相机旋转
    /// </summary>
    private void CameraTargetRotation()
    {
        //如果输入大于阈值
        if (_inputLook.sqrMagnitude >= _threshold)
        {
            _camTargetYaw += _inputLook.x;
            _camTargetPitch += _inputLook.y * (isFilpPitch ? -1 : 1);
        }
        //限制相机角度
        _camTargetYaw = TransformTools.ClampAngle(_camTargetYaw, float.MinValue, float.MaxValue);
        _camTargetPitch = TransformTools.ClampAngle(_camTargetPitch, bottomClamp, topClamp);
        //移动相机目标点
        followTarget.rotation = Quaternion.Euler(_camTargetPitch, _camTargetYaw, 0);
    }

    private void OnDrawGizmosSelected()
    {
        //如果在地面为绿色，如果不在为红色
        Color groundedColorGreen = new Color(0, 1, 0, 0.35f);
        Color airColorRed = new Color(1, 0, 0, 0.35f);
        Gizmos.color = isGrounded ? groundedColorGreen : airColorRed;

        //画球
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        Gizmos.DrawSphere(spherePosition, groundRadius);
    }

}
