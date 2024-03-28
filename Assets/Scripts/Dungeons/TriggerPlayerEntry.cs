using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerEntry : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] Animator DoorEntry;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemySpawner.StartSpawnCoroutine();
            DoorEntry.SetBool("isOpen?", false);
            this.gameObject.SetActive(false);
        }
    }
}
