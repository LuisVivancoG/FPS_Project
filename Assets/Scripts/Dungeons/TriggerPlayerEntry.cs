using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerEntry : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] Animator DoorEntry;
    [SerializeField] AudioManager audioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioManager.Play("Music");
            DoorEntry.SetBool("isOpen?", false);
            enemySpawner.StartSpawnCoroutine();
            this.gameObject.SetActive(false);
        }
    }
}
