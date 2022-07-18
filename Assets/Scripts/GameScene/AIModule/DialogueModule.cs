using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueModule : MonoBehaviour
{
    public List<TextAsset> dialogueList;

    private TextAsset nowDialogue;
    private bool isEnterAI = false;

    private UnityAction<KeyCode> inputEvent;
    private UnityAction aiEvent;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();

        //添加退出AI事件监听
        aiEvent = () => { isEnterAI = false; };
        EventCenter.Instance.AddListener("ExitAIModule", aiEvent);

        //添加按键说话事件监听
        inputEvent = (key) =>
        {
            //如果按下E
            if (key == KeyCode.E)
            {
                //如果有提示面板就隐藏
                UIMgr.Instance.HidePanel("GameTipPanel");

                //如果没有正在对话，就开启对话
                if (!DialogueMgr.Instance.isTalk && !isEnterAI)
                {
                    DialogueMgr.Instance.StartDialogue(nowDialogue);
                }
                //如果正在对话，开启下一句对话
                else
                {
                    DialogueMgr.Instance.NextSentence(null, () =>
                    {
                        EventCenter.Instance.PostEvent<GameObject>("EnterAIModule", this.gameObject);
                        isEnterAI = true;
                    });
                }
            }
        };
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener("ExitAIModule", aiEvent);
        EventCenter.Instance.RemoveListener("GetKeyDown", inputEvent);
    }

    /// <summary>
    /// 改变当前对话文本
    /// </summary>
    /// <param name="index">文本在List中的index</param>
    public void ChangeNowDialogue(int index)
    {
        nowDialogue = dialogueList[index];
    }

    //进入对话范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //做出反应动画
            anim.SetTrigger("Reaction");
            //开启提示面板，提示按键对话
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("按E开始对话");
            });
            //添加按键开启对话监听
            EventCenter.Instance.AddListener("GetKeyDown", inputEvent);
        }
    }

    //离开对话范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterAI = false;
            UIMgr.Instance.HideAllPanel("GamePanel");
            DialogueMgr.Instance.StopDialogue();
            //移除按键开启对话监听
            EventCenter.Instance.RemoveListener("GetKeyDown", inputEvent);
        }
    }
}
