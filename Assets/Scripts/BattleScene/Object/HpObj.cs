using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpObj : MonoBehaviour
{
    public Image imgHp;
    public Image imgHpBk;
    public float activeTime = 1;
    private float nowTime;
    public void UpdateHp(float hp, float maxHp)
    {
        imgHp.gameObject.SetActive(true);
        imgHpBk.gameObject.SetActive(true);
        imgHp.fillAmount = hp / maxHp;
        nowTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - nowTime > activeTime)
        {
            imgHp.gameObject.SetActive(false);
            imgHpBk.gameObject.SetActive(false);
        }
        transform.LookAt(Camera.main.transform.position);
    }
}
