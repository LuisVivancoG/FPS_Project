using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TankEnemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] int DmgTaken;
    [SerializeField] int AttackDamage;
    [SerializeField] float MoveSpeed;
    [SerializeField] int MoveSpeedMultiplier;
    Transform Target;
    [SerializeField] float ChargeRange;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float ReactionSpeed;
    [SerializeField] float ChannelChargeAtk;
    bool targetInRange;
    Vector3 lastPlayerPosition;
    [SerializeField] Image HealthBar;

    PlayerController Player;
    EnemyManager enemyManager;

    [SerializeField] Animator animator;
    [SerializeField] AgentLinkMover LinkMover;
    [SerializeField] Collider EnemyCollider;
    AudioManager audioManager;

    private void Awake()
    {
        Player = FindAnyObjectByType<PlayerController>();
        Target = Player.transform;

        enemyManager = FindAnyObjectByType<EnemyManager>();

        LinkMover.OnLinkStart += HandleLinkStart;

        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ChargeToTarget());
    }

    // Update is called once per frame
    void Update()
    {
        targetInRange = Vector3.Distance(transform.position, Target.transform.position) <= ChargeRange;


        if (Hp > 0)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (targetInRange)
        {
            animator.SetBool("isWalking?", false);
            lastPlayerPosition = Player.transform.position;
            StartCoroutine(ChargeToTarget());
        }
        else
        {
            StartCoroutine(MoveToTarget());
        }
    }

    void HandleLinkStart()
    {
        animator.SetTrigger("isJumping");
    }

    private IEnumerator ChargeToTarget()
    {
        animator.SetTrigger("onRange?");
        audioManager.Play("Rage");
        agent.isStopped = true;
        agent.autoBraking = false;
        agent.speed = MoveSpeed * MoveSpeedMultiplier;
        agent.acceleration = 20;
        agent.SetDestination(lastPlayerPosition);
        yield return new WaitForSeconds(ChannelChargeAtk);

        agent.isStopped = false;
    }

    private IEnumerator MoveToTarget()
    {
        animator.SetBool("isWalking?", true);
        agent.speed = MoveSpeed;
        agent.autoBraking = true;
        agent.acceleration = 8;
        agent.SetDestination(Target.transform.position);
        yield return new WaitForSeconds(ReactionSpeed);
    }

    IEnumerator Death()
    {
        animator.SetTrigger("isDead?");
        animator.SetBool("isWalking?", false);
        agent.isStopped = true;
        yield return new WaitForSeconds(2);
        agent.enabled = false;
        enemyManager.IncrementKillsCount();
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ChargeRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets")
        {
            audioManager.Play("PiranhaHit");
            HealthBar.fillAmount -= (HealthBar.fillAmount / (Hp / DmgTaken));
            Hp -= DmgTaken;
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
            EnemyCollider.enabled = false;
            StartCoroutine(Death());
        }
    }
    private void OnEnable()
    {
        EnemyCollider.enabled = true;
        agent.isStopped = false;
        agent.speed = MoveSpeed;
        agent.enabled = true;
        HealthBar.fillAmount = 1;
    }
}
