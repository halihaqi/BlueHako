using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public Text txtLoading;
    public Image imgProgress;

    private UnityAction<int> progressEvent;

    public override void ShowMe()
    {
        base.ShowMe();
        StartCoroutine(Loading());
        customEvent = () => { UIMgr.Instance.HidePanel("LoadingPanel"); };
        progressEvent = (pro) => { imgProgress.fillAmount = (float)pro / 100; };
        EventCenter.Instance.AddListener("LoadComplete", customEvent);
        EventCenter.Instance.AddListener("Loading", progressEvent);
    }

    public override void HideMe()
    {
        base.HideMe();
        EventCenter.Instance.RemoveListener("LoadComplete", customEvent);
        EventCenter.Instance.RemoveListener("Loading", progressEvent);
    }

    IEnumerator Loading()
    {
        while(txtLoading.text.Length < 3)
        {
            txtLoading.text += ".";
            yield return new WaitForSeconds(0.3f);
            if (txtLoading.text.Length == 3)
            {
                txtLoading.text = "";
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
