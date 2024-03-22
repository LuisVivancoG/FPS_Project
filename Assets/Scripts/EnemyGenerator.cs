using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int lenght;
    [SerializeField] GameObject[] tilesOptionsArray;

    GameObject enemyGridSpawner;

    Vector3[] tilesPositions;
    int tilesAvailable;
    Vector3 singleTilePosiition;

    [SerializeField] GameObject[] enemiesOptions;
    public int TotalEnemies;
    public int CurrentEnemies = 0;

    [Range(0f, 10f)]
    [SerializeField] float spawnDelay;

    [SerializeField] int obstaclesNumber;
    [SerializeField] GameObject[] ObstaclesOptions;

    public bool timerUp;
    Coroutine spawningCoroutine;
    [SerializeField] bool coroutineStarted = false;


    void OnEnable()
    {
        enemyGridSpawner = this.gameObject;
    }

    void Start()
    {
        DrawGrid();
        //GenerateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentEnemies == totalEnemies)
        //{
        //    coroutineStarted = false;
        //}
    }

    void DrawGrid()
    {
        Vector3 _OriginPos = gameObject.transform.position; // Set the initial position equal to the position of the object with this script
        float _xOffset = 10f;
        float _zOffset = 10f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < lenght; z++)
            {
                int _randomTile = UnityEngine.Random.Range(0, tilesOptionsArray.Length); //Pick random number from the array

                Vector3 _tilePosition = new Vector3(_OriginPos.x + x * _xOffset, _OriginPos.y, _OriginPos.z + z * _zOffset);
                GameObject _tileInstance = Instantiate(tilesOptionsArray[_randomTile], _tilePosition, transform.rotation); //Instanciate the index picked

                _tileInstance.transform.SetParent(enemyGridSpawner.transform); //Assign each tile as child from spawner
            }
        }

        tilesAvailable = transform.childCount; //Assign the tilesAvailable variable the value of the # of children on this gameObject
        tilesPositions = new Vector3[tilesAvailable]; //Include all positions from the children in the tilePositions array

        for (int i = 0; i < tilesAvailable; i++)
        {
            tilesPositions[i] = transform.GetChild(i).position; //Set each position of the children as a value for each index in the array
        }

        NavMeshSurface[] _navMeshSurfaces = GetComponents<NavMeshSurface>();

        foreach (NavMeshSurface navMeshSurface in _navMeshSurfaces)
        {
            navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navMeshSurface.collectObjects = CollectObjects.Children;
            //navMeshSurface.

            navMeshSurface.BuildNavMesh();
        }
    }

    IEnumerator SpawnEnemies() //Method needs to be coroutine because only coroutines can use "WaitForSeconds"
    {
        while (timerUp == false) //Do loop while timer is false
        {
            for (CurrentEnemies = 0; CurrentEnemies < TotalEnemies; CurrentEnemies++) //Instantiate the enemies needed
            {
                {
                int _randomPos = UnityEngine.Random.Range(0, tilesAvailable);
                int _randomEnemy = UnityEngine.Random.Range(0, enemiesOptions.Length);
                singleTilePosiition = tilesPositions[_randomPos];

                GameObject _enemyInstance = Instantiate(enemiesOptions[_randomEnemy], singleTilePosiition, transform.rotation); //Instantiate a random enemy at a random tile

                yield return new WaitForSeconds(spawnDelay);
                }
                timerUp = true;
            }
        }
    }

    public void StopSpawning()
    {
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }
    }

    public void StartSpawning()
    {
        if (coroutineStarted == false)
        {
            spawningCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    /*void GenerateObstacles()
    {
        for (int x = 0; x < obstaclesNumber; x++) //Instantiate the enemies needed
        {
            int _randomPos = UnityEngine.Random.Range(0, tilesAvailable);
            int _randomObstacle = UnityEngine.Random.Range(0, ObstaclesOptions.Length);
            singleTilePosiition = tilesPositions[_randomPos];

            GameObject _enemyInstance = Instantiate(ObstaclesOptions[_randomObstacle], new Vector3(singleTilePosiition.x, singleTilePosiition.y + 1, singleTilePosiition.z), Quaternion.Euler(0, 180, 0)); //Instantiate a random enemy at a random tile
        }
    }*/
}
