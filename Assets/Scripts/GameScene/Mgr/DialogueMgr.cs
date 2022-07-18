using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueMgr : Singleton<DialogueMgr>
{
    public bool isTalk = false;
    private Queue<string> dialogue = new Queue<string>();

    /// <summary>
    /// �����Ի���ѹ��Ի�����ʾ��һ��Ի�
    /// </summary>
    /// <param name="info">�Ի�����</param>
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
    /// �л�����һ��Ի�
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
    /// �����Ի����رնԻ���
    /// </summary>
    public void StopDialogue()
    {
        dialogue.Clear();
        isTalk = false;
        UIMgr.Instance.HidePanel("DialoguePanel");
        //�㲥�Ի�����
        EventCenter.Instance.PostEvent("DialogueOver");
    }

    /// <summary>
    /// �Ի�������ʾЭ��
    /// </summary>
    /// <param name="sentence">��ǰ����</param>
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
