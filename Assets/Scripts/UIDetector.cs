using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDetector : MonoBehaviour
{
    [SerializeField] UIControls uIControls;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            uIControls.GalleryUI();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uIControls.GameplayUI();
        }
    }
}
