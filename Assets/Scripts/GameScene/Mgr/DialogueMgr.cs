using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueMgr : Singleton<DialogueMgr>
{
    public bool isTalk = false;
    private Queue<string> dialogue = new Queue<string>();

    /// <summary>
    /// 开启对话框，压入对话，显示第一句对话
    /// </summary>
    /// <param name="info">对话数据</param>
    public void StartDialogue(TextAsset text, UnityAction callBack = null)
    {
        isTalk = true;
        dialogue.Clear();
        string str = text.text;
        string[] dialogues = str.Split('\n');
        foreach (string dia in dialogues)
        {
            dialogue.Enqueue(dia);
        }
        UIMgr.Instance.ShowPanel<DialoguePanel>("DialoguePanel", E_UI_Layer.Bot, (panel) =>
        {
            panel.txtName.text = text.name;
            NextSentence(callBack);
        });
    }

    /// <summary>
    /// 切换到下一句对话
    /// </summary>
    public void NextSentence(UnityAction callBack = null, UnityAction stopCallBack = null)
    {
        if (dialogue.Count <= 0)
        {
            if (isTalk)
            {
                StopDialogue();
                stopCallBack?.Invoke();
            }
            return;
        }
        MonoMgr.Instance.StopAllCoroutines();
        callBack?.Invoke();
        MonoMgr.Instance.StartCoroutine(ShowSentence(dialogue.Dequeue()));
    }

    /// <summary>
    /// 结束对话，关闭对话框
    /// </summary>
    public void StopDialogue()
    {
        dialogue.Clear();
        isTalk = false;
        UIMgr.Instance.HidePanel("DialoguePanel");
        //广播对话结束
        EventCenter.Instance.PostEvent("DialogueOver");
    }

    /// <summary>
    /// 对话逐字显示协程
    /// </summary>
    /// <param name="sentence">当前句子</param>
    /// <returns></returns>
    IEnumerator ShowSentence(string sentence)
    {
        DialoguePanel panel = UIMgr.Instance.GetPanel<DialoguePanel>("DialoguePanel");
        panel.txtDialogue.text = "";
        foreach (char item in sentence.ToCharArray())
        {
            panel.txtDialogue.text += item;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
    }
}
