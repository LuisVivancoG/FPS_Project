using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public bool availability = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemies")
        {
            availability = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemies")
        {
            availability = true;
        }
    }
}
