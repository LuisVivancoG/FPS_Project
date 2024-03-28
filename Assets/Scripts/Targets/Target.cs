using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] TargetsManager targetsManager;

    private void Awake()
    {
        targetsManager = FindAnyObjectByType<TargetsManager>();
    }

    private void OnCollisionEnter()
    {
        targetsManager.TargetsCount();
        gameObject.SetActive(false);
    }
}
