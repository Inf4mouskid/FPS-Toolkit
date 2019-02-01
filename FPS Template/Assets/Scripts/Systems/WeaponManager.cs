using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using TMPro;

[System.Serializable]
public struct FireArm
{
    [Header("Weapon")]
    public GameObject Weapon;
    [Tooltip("Weapon name")] public string Name;
    [Tooltip("UI graphic of weapon")] public Image WeaponGraphic;
    [Tooltip("UI graphic of weapon clip/mag")] public Image ClipGraphic;
    [Header("Physics System")]
    public bool EnableHitscan;
    public bool EnableProjectile;
    [Header("Stats")]
    [Range(10, 200)] public float WeaponRange;
    [Range(1, 100)] public int Damage;
}

public class WeaponManager : MonoBehaviour
{
    public FireArm[] FireArms;

    void Awake()
    {
        foreach (var FA in FireArms)
        {
            //Get weapon shot physics
            var Hitscan = FA.Weapon.GetComponent<Gun_Hitscan>();
            var Projectile = FA.Weapon.GetComponent<Gun_Projectile>();

            //
            Hitscan.enabled = FA.EnableHitscan;
            Projectile.enabled = FA.EnableProjectile;

            //
            if (Hitscan != null && Hitscan.enabled)
                Hitscan.GunDamage = FA.Damage;
            if (Projectile != null && Projectile.enabled)
                Projectile.GunDamage = FA.Damage;
        }
    }
}
