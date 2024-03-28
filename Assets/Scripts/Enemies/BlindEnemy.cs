using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BlindEnemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] int AttackDamage;
    [SerializeField] float MoveSpeed;
    [SerializeField] float FleeSpeed;
    Transform Target;
    NavMeshAgent agent;
    [SerializeField] float ReactionSpeed;
    [SerializeField] float AttackRange;
    bool TargetInRange;
    bool Scared;
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
        if (Scared == false)
        {
            Movement();
        }
        else
        {
            RunAway();
        }
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

    void RunAway()
    {
        agent.speed = FleeSpeed;
        Vector3 safeDistance = transform.position - Player.transform.position;
        Vector3 newPos = transform.position + safeDistance;
        agent.SetDestination(newPos);
    }

    void CheckHP()
    {
        if (Hp <= 10)
        {
            int flee = UnityEngine.Random.Range(1, 16);
            if (flee >= 12)
            {
                Scared = true;
                if (Scared)
                {
                    RunAway();
                }
            }
            else
            {
                Scared= false;
                Movement();
            }
        }

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
