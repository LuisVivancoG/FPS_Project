using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject EnemiesCalledGrp;
    [SerializeField] Text CrocosUI;
    [SerializeField] UIControls uIControls;
    int enemiesSpawned;
    int enemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        enemiesSpawned = enemySpawner.EnemiesNeeded;
        CrocosUI.text = (enemiesKilled + " / " + enemiesSpawned);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void IncrementKillsCount()
    {
        enemiesKilled++;
        CrocosUI.text = (enemiesKilled + " / " + enemiesSpawned);
        if (enemiesKilled >= enemiesSpawned)
        {
            uIControls.WinUI();
        }
    }
}
