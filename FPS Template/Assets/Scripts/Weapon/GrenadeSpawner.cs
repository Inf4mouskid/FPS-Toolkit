using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    public float ThrowForce = 40f;
    public GameObject Nade;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetAxis("Fire2") >= 0.5f)
        {
            Throw();
        }
    }

    void Throw()
    {
        var Grenade = Instantiate(Nade, transform.position, transform.rotation);
        var rb = Grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * ThrowForce, ForceMode.VelocityChange);
    }
}
