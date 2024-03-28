using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BuildDungeon : MonoBehaviour
{
    [SerializeField] int Width;
    [SerializeField] int Lenght;
    [SerializeField] GameObject[] TilesOptions;
    [SerializeField] GameObject TilesBuiltGrp;
    [SerializeField] float TilesSizeX;
    [SerializeField] float TilesSizeZ;

    // Start is called before the first frame update

    void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        Vector3 originPos = gameObject.transform.position; // Set the initial position equal to the position of the object with this script

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Lenght; z++)
            {
                int randomTile = UnityEngine.Random.Range(0, TilesOptions.Length); //Pick random number from the array
                int RandomRot = 90 * UnityEngine.Random.Range(0, 4);

                Vector3 tilePosition = new Vector3(originPos.x + x * TilesSizeX, originPos.y, originPos.z + z * TilesSizeZ);
                GameObject tileInstance = Instantiate(TilesOptions[randomTile], tilePosition, Quaternion.Euler(0, RandomRot, 0)); //Instanciate the index picked

                tileInstance.transform.SetParent(TilesBuiltGrp.transform); //Assign each tile as child from spawner
            }
        }
        NavMeshSurface[] navMeshes = TilesBuiltGrp.GetComponents<NavMeshSurface>();

        foreach (NavMeshSurface navMeshSurface in navMeshes)
        {
            navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navMeshSurface.collectObjects = CollectObjects.Children;
            navMeshSurface.BuildNavMesh();
        }
    }
}
