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
    public float topClamp = 70;//����������
    public float bottomClamp = -30;//�����󸩽�
    public bool isFilpPitch = true;

    [Header("GroundCheck")]
    public bool isGrounded = true;
    public float groundOffset = -0.29f;
    public float groundRadius = 0.28f;
    public LayerMask groundLayers = 1;

    //�������
    private Vector2 _inputLook;
    private Vector2 _inputMove;
    private float _threshold = 0.01f;//��������ż�

    //�ƶ�����
    private float _targetRot = 0;
    private float _rotVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53;

    //�������
    private Camera followCamera;
    private Transform followTarget;
    private float _camTargetYaw;
    private float _camTargetPitch;

    //�����¼�����
    protected UnityAction<KeyCode> inputEvent;

    //Component
    protected Animator anim;
    protected CharacterController cc;

    protected virtual void Awake()
    {
        //��ȡ���
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        //�ҵ���������
        followTarget = transform.Find("FollowPos");
        followCamera = Camera.main;
        followCamera.GetComponent<Player_Cam>().followTarget = followTarget;
        //���������
        InputMgr.Instance.OpenOrClose(true);
        //����B�򿪻�رձ���
        //����Tab�������б�
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
        //�ر��������
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
    /// �����������
    /// </summary>
    private void UpdateInput()
    {
        _inputLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// �ƶ���ת��
    /// </summary>
    private void Move()
    {
        //�ж��Ƿ�Ϊ����ٶ�
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        //�ж��ƶ������Ƿ�Ϊ0
        if (_inputMove == Vector2.zero)
            targetSpeed = 0;

        //����ķ���
        Vector3 inputDir = new Vector3(_inputMove.x, 0, _inputMove.y);

        //�����ƶ�ʱ�İ�����ת
        if(_inputMove != Vector2.zero)
        {
            _targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + followCamera.transform.eulerAngles.y;
            float rot = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRot, ref _rotVelocity, rotateSmoothTime);

            transform.rotation = Quaternion.Euler(0, rot, 0);
        }

        //��ת����ƶ�����
        Vector3 targetDir = Quaternion.Euler(0, _targetRot, 0) * Vector3.forward;

        //�ƶ�����,���ϴ�ֱ�ٶ�
        cc.Move((targetDir.normalized + Vector3.up * _verticalVelocity) * targetSpeed * Time.deltaTime);
        //�ƶ�����
        anim.SetFloat("Speed", Input.GetKey(KeyCode.LeftShift) ? inputDir.magnitude : inputDir.magnitude / 2);
    }

    /// <summary>
    /// ������
    /// </summary>
    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Gravity()
    {
        if (!isGrounded)
        {
            //���û�дﵽ��ֱ�ٶ���ֵ���ͻ�һֱ�Ӵ�ֱ�ٶ�
            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = 0;
        }
    }

    /// <summary>
    /// �����˳������ת
    /// </summary>
    private void CameraTargetRotation()
    {
        //������������ֵ
        if (_inputLook.sqrMagnitude >= _threshold)
        {
            _camTargetYaw += _inputLook.x;
            _camTargetPitch += _inputLook.y * (isFilpPitch ? -1 : 1);
        }
        //��������Ƕ�
        _camTargetYaw = TransformTools.ClampAngle(_camTargetYaw, float.MinValue, float.MaxValue);
        _camTargetPitch = TransformTools.ClampAngle(_camTargetPitch, bottomClamp, topClamp);
        //�ƶ����Ŀ���
        followTarget.rotation = Quaternion.Euler(_camTargetPitch, _camTargetYaw, 0);
    }

    private void OnDrawGizmosSelected()
    {
        //����ڵ���Ϊ��ɫ���������Ϊ��ɫ
        Color groundedColorGreen = new Color(0, 1, 0, 0.35f);
        Color airColorRed = new Color(1, 0, 0, 0.35f);
        Gizmos.color = isGrounded ? groundedColorGreen : airColorRed;

        //����
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        Gizmos.DrawSphere(spherePosition, groundRadius);
    }

}
