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

        //����˳�AI�¼�����
        aiEvent = () => { isEnterAI = false; };
        EventCenter.Instance.AddListener("ExitAIModule", aiEvent);

        //��Ӱ���˵���¼�����
        inputEvent = (key) =>
        {
            //�������E
            if (key == KeyCode.E)
            {
                //�������ʾ��������
                UIMgr.Instance.HidePanel("GameTipPanel");

                //���û�����ڶԻ����Ϳ����Ի�
                if (!DialogueMgr.Instance.isTalk && !isEnterAI)
                {
                    DialogueMgr.Instance.StartDialogue(nowDialogue);
                }
                //������ڶԻ���������һ��Ի�
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
    /// �ı䵱ǰ�Ի��ı�
    /// </summary>
    /// <param name="index">�ı���List�е�index</param>
    public void ChangeNowDialogue(int index)
    {
        nowDialogue = dialogueList[index];
    }

    //����Ի���Χ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //������Ӧ����
            anim.SetTrigger("Reaction");
            //������ʾ��壬��ʾ�����Ի�
            UIMgr.Instance.ShowPanel<GameTipPanel>("GameTipPanel", E_UI_Layer.System, (panel) =>
            {
                panel.ChangeInfo("��E��ʼ�Ի�");
            });
            //��Ӱ��������Ի�����
            EventCenter.Instance.AddListener("GetKeyDown", inputEvent);
        }
    }

    //�뿪�Ի���Χ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnterAI = false;
            UIMgr.Instance.HideAllPanel("GamePanel");
            DialogueMgr.Instance.StopDialogue();
            //�Ƴ����������Ի�����
            EventCenter.Instance.RemoveListener("GetKeyDown", inputEvent);
        }
    }
}
