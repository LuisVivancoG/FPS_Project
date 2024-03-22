using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] int forceSpeed;
    [SerializeField] int damage;

    void OnEnable()
    {
        Shoot();
    }

    private void OnDisable()
    {
    }

    void Shoot()
    {
        var _physicsInstance = GetComponent<Rigidbody>();
        _physicsInstance.velocity = Vector3.zero;
        _physicsInstance.AddForce(transform.forward * forceSpeed); //Find another way to impulse the bullets cause AddForce accumulates each time gets called, thus it is hard to controll it.
    }

    private void OnCollisionEnter()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Limits")
        {
            gameObject.SetActive(false);
        }
    }
}
