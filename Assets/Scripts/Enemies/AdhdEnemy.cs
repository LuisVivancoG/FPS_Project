using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AdhdEnemy : MonoBehaviour
{
    [SerializeField] int Hp;
    [SerializeField] int DmgTaken;
    [SerializeField] int AttackDamage;
    [SerializeField] float MoveSpeed;
    Transform Target;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float ReactionSpeed;
    [SerializeField] float AttackRange;
    bool TargetInRange;
    [SerializeField] Image HealthBar;
    PlayerController Player;
    Vector3 lastPlayerPosition;
    [SerializeField] Animator animator;
    [SerializeField] AgentLinkMover LinkMover;
    [SerializeField] Collider EnemyCollider;

    EnemyManager enemyManager;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        TargetInRange = Vector3.Distance(transform.position, Target.transform.position) <= AttackRange;

        if (Hp > 0)
        {
            Movement();
        }
    }

    void Movement()
    {        
        if (TargetInRange)
        {
            animator.SetBool("isWalking?", false);
            lastPlayerPosition = Player.transform.position;
            StartCoroutine(AttackTarget());
        }
        else
        {
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator AttackTarget()
    {
        animator.SetTrigger("onRange?");
        audioManager.Play("CrocoBite");
        agent.isStopped = true;
        yield return new WaitForSeconds(ReactionSpeed);
        agent.isStopped = false;
    }

    private IEnumerator MoveToTarget()
    {
        animator.SetBool("isWalking?", true);
        agent.SetDestination(Target.transform.position);
        yield return new WaitForSeconds(ReactionSpeed);
    }
    IEnumerator Death()
    {
        animator.SetTrigger("isDead?");
        animator.SetBool("isWalking?", false);
        enemyManager.IncrementKillsCount();
        agent.isStopped = true;
        yield return new WaitForSeconds(2);
        agent.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    void HandleLinkStart()
    {
        animator.SetTrigger("isJumping");
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
