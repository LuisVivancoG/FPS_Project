using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableProp : MonoBehaviour
{
    [SerializeField] GameObject OriginalProp;
    [SerializeField] GameObject DestroyedProp;
    [SerializeField] Collider PropCollider;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullets" || other.gameObject.tag == "EnemyTank")
        {
            OriginalProp.SetActive(false);
            PropCollider.enabled = false;
            DestroyedProp.SetActive(true);
        }
    }
    //public IEnumerator Destruction()
    //{
    //    yield return new WaitForSeconds(1);
    //}
}
