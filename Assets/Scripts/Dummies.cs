using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummies : MonoBehaviour
{
    GameplayUI gameplayUI;

    private void OnEnable()
    {
        gameplayUI = FindAnyObjectByType<GameplayUI>();
    }
    private void OnCollisionEnter()
    {
        gameplayUI.txtTargetsDestroyed++;
        gameObject.SetActive(false);
    }
}
