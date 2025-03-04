using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Damage,
    Die
}
public class EnemyAi : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    [SerializeField] float traceDistance = 10.0f;
    [SerializeField] float attackDistacne = 5.0f;
    [SerializeField] int HP = 10;

    bool isDie = false;
    EnemyState state = EnemyState.Idle;

    Animator anim;
    PlayerMovement playerMove;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerMove = player.GetComponent<PlayerMovement>();

        StartCoroutine(CheckEnemy());
        StartCoroutine(ChangeAction());
    }

    void Update()
    {
        if(isDie == true)
        {
            StartCoroutine(AfterDie());
            isDie = false;
        }
    }


    IEnumerator CheckEnemy()
    {
        while(isDie == false)
        {
            if (HP <= 0)
            {
                isDie = true;
            }

            yield return new WaitForSeconds(0.3f);

            float distance = Vector3.Distance(transform.position,player.position);

            if(distance <= attackDistacne)
            {
                state = EnemyState.Attack;
            }

            else if(distance <= traceDistance)
            {
                state = EnemyState.Move;
            }

            else
            {
                state = EnemyState.Idle;
            }
        }
    }

    IEnumerator ChangeAction()
    {
        while(isDie == false)
        {
            if (HP <= 0)
            {
                isDie = true;
            }

            switch (state)
            {
                case EnemyState.Idle:
                    agent.isStopped = true;
                    //anim.SetBool("isTrace", false);
                    anim.SetBool("isAttack", false);
                    break;
                case EnemyState.Move:
                    agent.SetDestination(player.position);
                    agent.isStopped = false;
                    anim.SetBool("isTrace", true);
                    anim.SetBool("isAttack", false);
                    break;
                case EnemyState.Attack:
                    transform.rotation = Quaternion.LookRotation(player.position);
                    //anim.SetBool("isTrace", false);
                    anim.SetBool("isAttack", true);
                    break;
                case EnemyState.Die:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator AfterDie()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        playerMove.ClearEnemyNum();
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Weapon"))
        {
            anim.SetTrigger("Damage");
            transform.rotation = Quaternion.LookRotation(player.position);
            HP -= playerMove.ThrowAttack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Weapon"))
        {
            anim.SetTrigger("Damage");
            transform.rotation = Quaternion.LookRotation(player.position);
            HP -= playerMove.NormalAttack();
        }
    }

    private void OnDrawGizmos()
    {
        if(state == EnemyState.Move)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDistance);
        }

        if (state == EnemyState.Attack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistacne);
        }
    }
}
