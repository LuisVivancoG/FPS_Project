using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLimit : MonoBehaviour
{
    [SerializeField] EnemyGenerator EnemiesSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnemiesSpawner.timerUp = false;
            EnemiesSpawner.StartSpawning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnemiesSpawner.timerUp = true;
            EnemiesSpawner.StopSpawning();
        }
    }
}
