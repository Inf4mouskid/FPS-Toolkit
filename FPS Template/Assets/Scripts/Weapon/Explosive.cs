using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [Tooltip("Special effect to be used on explosion")]
    public GameObject BlastEffect;

    [Range(1f, 60f)] public float DelayTimer = 3f;
    [Range(1f, 100f)] public float BlastRadius = 5f;
    [Range(100, 1000)] public int ExplosionForce = 200;
    [Range(1f, 100f)] public float Damage = 10f;

    private bool HasExploded = false;
    private float countdown;

    // Use this for initialization
    void Start()
    {
        countdown = DelayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        Debug.Log("Countdown: " + (int)countdown);
        if (countdown <= 0f && !HasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (BlastEffect != null)
        {
            var BlastObj = Instantiate(BlastEffect, transform.position, transform.rotation);
            Destroy(BlastObj, 1f);
        }

        //Colliders that will be destroyed or hurt by explosive force
        Collider[] collidersToDestroyAndHurt = Physics.OverlapSphere(transform.position, BlastRadius);

        foreach (var nearbyObject in collidersToDestroyAndHurt)
        {
            Debug.Log(nearbyObject.name);
            // var HealthStat = nearbyObject.GetComponent<Health_Guard>();
            var dest = nearbyObject.GetComponent<Destructible>();
            /*
                if (HealthStat != null)
                    HealthStat.TakeDamage(Damage);*/
            if (dest != null)
                dest.DestroyModel();
        }

        //Colliders that will be pushed by explosive force
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, BlastRadius);

        foreach (var nearbyObject in collidersToMove)
        {
            var rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, BlastRadius);
            }
        }

        Destroy(gameObject);
    }
}
