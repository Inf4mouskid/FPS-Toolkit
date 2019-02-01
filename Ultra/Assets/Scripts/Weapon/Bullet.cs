using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ImpactEffect;
    [Range(0f, 5f)] public float ImpactDeathTime = 1f;
    [Range(1, 100)] public int Damage = 1;

    //Action for bullet impact on an object
    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var WellnessStats = hit.GetComponent<Health_Guard>();

        // Check if the bullet has hit something
        if (hit != null && ImpactEffect != null)
        {
            var ImpactGO = Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(ImpactGO, ImpactDeathTime);
        }
        else if (hit != null && WellnessStats != null)
        {
            WellnessStats.TakeDamage(Damage);
        }

        //Check if target has taken damage to the head or body
        Destroy(gameObject);
    }
}
