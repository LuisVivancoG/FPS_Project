using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    //The GameObject carrying this script will be the origin for the grid. Set the origin at least half of the prefab units (2.5) 
    [SerializeField] int width;
    [SerializeField] int lenght;
    [SerializeField] GameObject[] tilesOptionsArray;

    GameObject targetGridSpawner;

    Vector3[] tilesPositions;
    int tilesAvailable;
    Vector3 singleTilePosiition;

    public int TotalTargets;
    [SerializeField] GameObject[] targetsOptions;

    void OnEnable()
    {
        targetGridSpawner = this.gameObject;
    }

    void Start()
    {
        DrawGrid();
        GenerateTargets();
    }

    void DrawGrid()
    {
        Vector3 _OriginPos = gameObject.transform.position; // Set the initial position equal to the position of the object with this script
        float _xOffset = 1f;
        float _zOffset = 1f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < lenght; z++)
            {
                int _randomTile = UnityEngine.Random.Range(0, tilesOptionsArray.Length); //Pick random number from the array

                Vector3 _tilePosition = new Vector3(_OriginPos.x + x * _xOffset, _OriginPos.y, _OriginPos.z + z * _zOffset);
                GameObject _tileInstance = Instantiate(tilesOptionsArray[_randomTile], _tilePosition, transform.rotation); //Instanciate the index picked

                _tileInstance.transform.SetParent(targetGridSpawner.transform); //Assign each tile as child from spawner
            }
        }

        tilesAvailable = transform.childCount; //Assign the tilesAvailable variable the value of the # of children on this gameObject
        tilesPositions = new Vector3[tilesAvailable]; //Include all positions from the children in the tilePositions array

        for (int i = 0; i < tilesAvailable; i++)
        {
            tilesPositions[i] = transform.GetChild(i).position; //Set each position of the children as a value for each index in the array
        }
    }

    void GenerateTargets()
    {
        for (int x = 0; x < TotalTargets; x++) //Instantiate the enemies needed
        {
            int _randomPos = UnityEngine.Random.Range(0, tilesAvailable);
            int _randomTarget = UnityEngine.Random.Range(0, targetsOptions.Length);
            singleTilePosiition = tilesPositions[_randomPos];

            GameObject _enemyInstance = Instantiate(targetsOptions[_randomTarget], new Vector3(singleTilePosiition.x, singleTilePosiition.y + 1, singleTilePosiition.z), Quaternion.Euler(0, 180, 0)); //Instantiate a random enemy at a random tile
            _enemyInstance.transform.SetParent(targetGridSpawner.transform);
        }
    }
}
