using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Guard : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Set the amount of Shield a character will start with")]
    [Range(10, 1000)] public int MaxShield = 100;
    [Tooltip("Set the amount of Health a target will have")]
    [Range(10, 1000)] public int MaxHealth = 100;
    [Tooltip("Set the wait time for when the target will heal")]
    [Range(1, 10)] public int WaitTime = 1;
    [Tooltip("Set the rate for when the target is fully healed")]
    [Range(0.1f, 10f)] public int RechargeRate = 1;
    [Tooltip("Set the time for how much target will heal")]
    [Range(0.1f, 10f)] public float RechargeTime = 0.5f;
    private bool TakingDamage = false;
    private int Shield;
    private int Health;
    private float countdown;


    // Use this for initialization
    private void Start()
    {
        Shield = MaxShield;
        Health = MaxHealth;
        countdown = WaitTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!TakingDamage && Shield < MaxShield)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                StartCoroutine(HealGO());
                countdown = WaitTime;
            }
        }
    }

    IEnumerator HealGO()
    {
        string DebugShield = "<color=Yellow>Shield</color>: ";
        string DebugHealth = " <color=Red>Health</color>: ";
        Debug.Log("Recharging...");
        Debug.Log(DebugShield + Shield + DebugHealth + Health);

        //Check if player health is low 
        if (Health < MaxHealth)
        {
            while (Health < MaxHealth)
            {
                yield return new WaitForSeconds(RechargeTime);
                Health += RechargeRate;
            }
            Health = MaxHealth;
            Debug.Log(DebugShield + Shield + DebugHealth + Health);
        }

        //Restore player shields
        while (Shield < MaxShield)
        {
            yield return new WaitForSeconds(RechargeTime);
            Shield += RechargeRate;
        }
        Shield = MaxShield;
        Debug.Log(DebugShield + Shield + DebugHealth + Health);
    }

    public void TakeDamage(int amount)
    {
        TakingDamage = true;
        countdown = WaitTime;
        Debug.Log("Damage amount: " + amount);
        if (Shield > 0)
        {
            Debug.Log("Damaging shield!");
            Shield -= amount;
            if (Shield < 0)
                Shield = 0;
        }
        else if (Shield == 0)
        {
            Debug.Log("Damaging health!");
            Health -= amount + 2;
            if (Health <= 0f)
            {
                var Dest = GetComponent<Destructible>();
                if (Dest != null)
                    Dest.DestroyModel();
                Destroy(gameObject);
            }
        }
        TakingDamage = false;
    }
}
