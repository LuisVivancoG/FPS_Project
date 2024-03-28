using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetsManager : MonoBehaviour
{
    [SerializeField] ShootingGallery shootingGallery;
    [SerializeField] float RestartTimer;
    int targetsDestroyed;

    [SerializeField] Text TargetsUI;

    [SerializeField] Animator DoorAnim;

    private void Awake()
    {
        TargetsUI.text = (targetsDestroyed + " / " + shootingGallery.TotalTargets);
    }

    public void TargetsCount()
    {
        targetsDestroyed++;
        TargetsUI.text = (targetsDestroyed + " / " + shootingGallery.TotalTargets);

        if (targetsDestroyed == shootingGallery.TotalTargets)
        {
            targetsDestroyed = 0;
            StartCoroutine(RestartGallery());

            if (DoorAnim.GetBool("isOpen?") == false)
            {
                DoorAnim.SetBool("isOpen?", true);
            }
        }
    }

    IEnumerator RestartGallery()
    {
        yield return new WaitForSeconds(RestartTimer);
        TargetsUI.text = (targetsDestroyed + " / " + shootingGallery.TotalTargets);
        shootingGallery.RestoreTargets();
    }
}
