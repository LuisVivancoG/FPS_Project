using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int damageRecieved;
    Transform playerLoc;
    NavMeshAgent agent;
    SkinnedMeshRenderer enemyRenderer;
    GameplayUI gameplayUI;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerLoc = FindAnyObjectByType<PlayerController>().transform;
        enemyRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        gameplayUI = FindAnyObjectByType<GameplayUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set HP system and get hit by bullets
        HpCheck();
        ChasePlayer();

        //Behaviour move towards player                                                                                                                 DONE
        //Give points to the player when enemy dies
        //If enough time play animations and maybe attack player                                                                                        DONE        
        //Create a manager script that makes a pool of enemies and picks randomly a enemy from the array. Set max number of enemies generated           DONE
        //Set a timer for the game mode                                                                                                                 1/2
        //set a points counter and maybe a enemies left counter
        //Create a class for different enemies with values like speed, health, dmg
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullets")
        {
            hp = hp - damageRecieved;
            enemyRenderer.material.color = Color.red;
            //Debug.Log("Current hp is " + hp);
        }
    }

    void HpCheck()
    {
        if (hp <= 0)
        {
            //gameObject.GetComponent<EnemyAI>().enabled = false;
            gameplayUI.txtCrocosKilled++;
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }

    void ChasePlayer()
    {
        agent.destination = playerLoc.position;
    }
}
