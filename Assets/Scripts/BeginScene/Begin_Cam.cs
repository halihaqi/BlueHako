using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin_Cam : SingletonMono<Begin_Cam>
{
    public float rotateSpeed = 10;
    private Vector2 mouseInput;
    private Vector3 startRot;
    private Vector3 frontRot;
    private Animator anim;

    private void Start()
    {
        startRot = transform.localEulerAngles;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //��һ֡ŷ���ǣ�����������ת�Ƕ�
        frontRot = transform.localEulerAngles;
        //�������
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //��תĿ��Ƕ�
        Quaternion targetRot = Quaternion.AngleAxis(mouseInput.x, Vector3.up) *
            Quaternion.AngleAxis(-mouseInput.y, Vector3.forward) * transform.rotation;
        //����z��ƫ��
        targetRot = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y, startRot.z);
        //��ת
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);


        //������ת�Ƕ�
        if (transform.localEulerAngles.y > startRot.y + 5
            || transform.localEulerAngles.y < startRot.y - 5)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                frontRot.y, transform.localEulerAngles.z);
        }

        if (transform.localEulerAngles.x > startRot.x + 5
            || transform.localEulerAngles.x < startRot.x - 5)
        {
            transform.localEulerAngles = new Vector3(frontRot.x,
                transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

    }

    public void Move()
    {
        anim.SetTrigger("Move");
    }

    public void MoveBack()
    {
        anim.SetTrigger("MoveBack");
    }
}
