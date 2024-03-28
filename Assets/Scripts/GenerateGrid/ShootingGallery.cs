using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingGallery : MonoBehaviour
{
    [SerializeField] int Width;
    [SerializeField] int Lenght;
    [SerializeField] GameObject[] TilesOptionsArray;

    [SerializeField] GameObject TilesGrp;
    [SerializeField] GameObject TargetGrp;

    Vector3[] tilesPositions;
    int tilesAvailable;
    Vector3 singleTilePosiition;

    public int TotalTargets;
    [SerializeField] List<GameObject> targetsPool;
    [SerializeField] GameObject[] TargetsOptions;

    void Start()
    {
        DrawGrid();
        GenerateTargets();
    }

    void DrawGrid()
    {
        Vector3 originPos = gameObject.transform.position; // Set the initial position equal to the position of the object with this script
        float xOffset = 5f;
        float zOffset = 5f;

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Lenght; z++)
            {
                int randomTile = UnityEngine.Random.Range(0, TilesOptionsArray.Length); //Pick random number from the array

                Vector3 tilePosition = new Vector3(originPos.x + x * xOffset, originPos.y, originPos.z + z * zOffset);
                GameObject tileInstance = Instantiate(TilesOptionsArray[randomTile], tilePosition, transform.rotation); //Instanciate the index picked

                tileInstance.transform.SetParent(TilesGrp.transform); //Assign each tile as child from spawner
            }
        }

        tilesAvailable = TilesGrp.transform.childCount; //Assign the tilesAvailable variable the value of the # of children on this gameObject
        tilesPositions = new Vector3[tilesAvailable]; //Include all positions from the children in the tilePositions array

        for (int i = 0; i < tilesAvailable; i++)
        {
            tilesPositions[i] = TilesGrp.transform.GetChild(i).position; //Set each position of the children as a value for each index in the array
        }
    }

    void GenerateTargets()
    {
        for (int x = 0; x < TotalTargets; x++) //Instantiate the enemies needed
        {
            int randomPos = UnityEngine.Random.Range(0, tilesAvailable);
            int randomTarget = UnityEngine.Random.Range(0, TargetsOptions.Length);
            singleTilePosiition = tilesPositions[randomPos];

            GameObject targetInstance = Instantiate(TargetsOptions[randomTarget], new Vector3(singleTilePosiition.x, singleTilePosiition.y + 1, singleTilePosiition.z), Quaternion.Euler(0, 180, 0)); //Instantiate a random enemy at a random tile
            targetInstance.transform.SetParent(TargetGrp.transform);

            targetsPool.Add(targetInstance);
        }
    }

    public void RestoreTargets()
    {
        for (int x = 0; x < targetsPool.Count; x++)
        {
            int randomPos = UnityEngine.Random.Range(0, tilesAvailable);
            singleTilePosiition = tilesPositions[randomPos];

            targetsPool[x].transform.position = new Vector3(singleTilePosiition.x, singleTilePosiition.y + 1, singleTilePosiition.z);
            targetsPool[x].SetActive(true);
        }
    }
}
