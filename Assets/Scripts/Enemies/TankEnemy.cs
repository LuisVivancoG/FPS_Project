using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] int AttackDamage;
    [SerializeField] float MoveSpeed;
    [SerializeField] int MoveSpeedMultiplier;
    Transform Target;
    [SerializeField] float ChargeRange;
    NavMeshAgent agent;
    [SerializeField] float ReactionSpeed;
    [SerializeField] bool TargetInChargeRange;
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
        //StartCoroutine(ChargeToTarget());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        TargetInChargeRange = Vector3.Distance(transform.position, Target.transform.position) <= ChargeRange;
        
        if (TargetInChargeRange)
        {
            StartCoroutine(ChargeToTarget());
        }
        else
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator ChargeToTarget()
    {
        agent.speed = MoveSpeed * MoveSpeedMultiplier;
        //agent.angularSpeed = 20;
        agent.autoBraking = false;
        agent.SetDestination(Target.transform.position);
        yield return new WaitForSeconds(ReactionSpeed);
        agent.speed = MoveSpeed;
        agent.autoBraking = true;
    }

    private IEnumerator MoveToTarget()
    {
        agent.speed = MoveSpeed;
        //agent.angularSpeed = 100;
        agent.SetDestination(Target.transform.position);
        yield return new WaitForSeconds(ReactionSpeed);
    }

        private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ChargeRange);
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
