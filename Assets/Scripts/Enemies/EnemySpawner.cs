using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BuildDungeon buildDungeon;
    [SerializeField] GameObject DungeonTilesBuilt;

    [SerializeField] GameObject EnemiesCreated;

    [SerializeField] int EnemiesNeeded;
    [SerializeField] float SpawnDelay;

    [System.Serializable]
    class EnemiesAvailable
    {
        [SerializeField] internal GameObject EnemyPrefab;
        [Range(0, 1)]
        [SerializeField] internal float WeightPerc;
    }

    [SerializeField] List<EnemiesAvailable> Enemies;

    List<GameObject> EnemiesSpawning = new List<GameObject>();

    float totalWeight;

    private void Awake()
    {
        foreach (EnemiesAvailable enemiesAvailable in Enemies)
        {
            totalWeight += enemiesAvailable.WeightPerc;
        }
    }

    private void Start()
    {
        ListEnemiesToSpawn();
    }

    internal void StartSpawnCoroutine()
    {
        StartCoroutine(SpawnEnemiesList());
    }

    IEnumerator SpawnEnemiesList()
    {
        int tilesAvailable = DungeonTilesBuilt.transform.childCount;

        foreach (GameObject enemy in EnemiesSpawning)
        {
            int randomPos = UnityEngine.Random.Range(0, tilesAvailable);
            Transform child = DungeonTilesBuilt.transform.GetChild(randomPos);
            enemy.transform.position = child.transform.position;
            enemy.SetActive(true);

            yield return new WaitForSeconds(SpawnDelay);
        }
        Debug.Log("FinishedCoroutine");
    }

    public void ListEnemiesToSpawn()
    {
        for (int i = 0; i < EnemiesNeeded; i++)
        {
            float randomNumber = Random.Range(0, totalWeight);

            GameObject RandomEnemy = FindRandomEnemyWeighted(randomNumber);
            if (RandomEnemy != null)
            {
                GameObject Instance = Instantiate(RandomEnemy);
                Instance.transform.SetParent(EnemiesCreated.transform);
                Instance.SetActive(false);
                EnemiesSpawning.Add(Instance);
            }
        }
    }

    private GameObject FindRandomEnemyWeighted(float randomNumber)
    {
        // Loop through the pool and check which object can be called with randomNumber
        foreach (EnemiesAvailable picked in Enemies)
        {
            if (randomNumber < picked.WeightPerc)
            {
                return picked.EnemyPrefab;
            }
            randomNumber -= picked.WeightPerc;
        }
        float newRandomNumber = Random.Range(0, totalWeight);
        return FindRandomEnemyWeighted(newRandomNumber);
    }
}
