using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdhdEnemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] int AttackDamage;
    [SerializeField] float MoveSpeed;
    Transform Target;
    NavMeshAgent agent;
    [SerializeField] float ReactionSpeed;
    [SerializeField] float AttackRange;
    [SerializeField] bool TargetInRange;
    //Animator animator;

    PlayerController Player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = FindAnyObjectByType<PlayerController>();
        //animator = GetComponent<Animator>();
        Target = Player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(MoveToTarget());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        TargetInRange = Vector3.Distance(transform.position, Target.transform.position) <= AttackRange;
        
        if (TargetInRange)
        {
            StartCoroutine(AttackTarget());
        }
        else
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator AttackTarget()
    {
        //animator.SetTrigger("isAttacking?");
        agent.speed = 0;
        yield return new WaitForSeconds(ReactionSpeed);
        agent.speed = MoveSpeed;
    }

    private IEnumerator MoveToTarget()
    {
        agent.speed = MoveSpeed;
        agent.SetDestination(Target.transform.position);
        yield return new WaitForSeconds(ReactionSpeed);
    }

        private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets")
        {
            Hp -= 10;
            CheckHP();
        }

        if (collision.gameObject.tag == "Player")
        {
            Player.TakeDamage(AttackDamage);
        }
    }

    void CheckHP()
    {
        if (Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
