using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCObj : MonoBehaviour
{
    public bool isPatrol = false;

    public float moveSpeed = 2f;
    public float rotateSpeed = 100;
    public float stopTime = 3;
    public float talkTime = 2;

    public List<Transform> patrolPos;

    private Animator anim;
    private NavMeshAgent agent;

    private Transform playerTrans;
    private Quaternion startRot;

    private void Awake()
    {
        startRot = transform.rotation;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetLayerWeight(1, 1);
        this.GetComponent<DialogueModule>()?.ChangeNowDialogue(0);
        agent.speed = moveSpeed;
        agent.angularSpeed = rotateSpeed;
    }

    private void Start()
    {
        if (isPatrol)
            agent.SetDestination(patrolPos[0].position);
    }

    private void Update()
    {
        if(playerTrans != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                    (playerTrans.position - transform.position), rotateSpeed * Time.deltaTime);
        }
        if (!isPatrol)
            return;
        anim.SetBool("Walk", agent.velocity != Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.isStopped = true;
            playerTrans = other.transform;
        }
        if (!isPatrol)
            return;
        if (other.CompareTag("NavTarget"))
        {
            StartCoroutine(DelayPathfinding(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopTalk());
        }
    }

    IEnumerator StopTalk()
    {
        yield return new WaitForSeconds(talkTime);
        playerTrans = null;
        agent.isStopped = false;
        transform.rotation = startRot;
    }

    IEnumerator DelayPathfinding(Collider other)
    {
        yield return new WaitForSeconds(stopTime);
        Vector3 targetPos = other.transform.position;
        while (targetPos == other.transform.position)
        {
            targetPos = patrolPos[Random.Range(0, patrolPos.Count)].position;
        }
        agent.SetDestination(targetPos);
    }

}
