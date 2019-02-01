using System.Collections;
using UnityEngine;

public class Gun_Hitscan : MonoBehaviour
{
    //Structs
    public enum WeaponMode { Auto, Burst, Semi }

    //Public Variables
    [Header("Components")] //Weapon components tag
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;
    public Animator ReticleAnimation;
    [Header("Configuration")] //Weapon configuration tag
    public WeaponMode GunMode;
    [Range(1, 100)] public int MaxAmmo = 100;
    [Range(1, 10)] public int MaxClips = 1;
    [Range(0, 10)] public int BulletSpreadSize = 0;
    [Header("Burst Settings")] //Weapon configuration tag
    [Range(2, 4)] public int BurstAmount = 3;
    [Range(0.03f, 0.1f)] public float BurstInterval = 0.1f;
    [Header("Stats")] // Weapon stats tag
    [Range(1, 10)] public int HitForce = 1;
    [Range(1f, 25f)] public float FireRate = 5f;
    [Range(1, 100)] public int GunDamage = 1;
    [Range(10.0f, 200.0f)] public float WeaponRange = 50f;
    [Range(0.01f, 2.0f)] public float ImpactDeathTime = 1.0f;
    [HideInInspector]
    [Header("Extras")] // Extras tag
    public Color DebugRayColor = Color.green;

    //Private Variables -- Unity
    private AudioSource GunAudio;
    private Camera FpsCamera;
    [SerializeField] private Vector3 ShootDirection;

    //Private Variables -- Language
    private bool hasShot = false;
    private float NextShot = 0;
    private float SpreadFactor;
    private int Ammo;
    private int ClipCount;


    //Get prerequsite data
    void Start()
    {
        GunAudio = GetComponent<AudioSource>();
        SpreadFactor = (float)BulletSpreadSize / 100;
        FpsCamera = GetComponentInParent<Camera>();
        Ammo = MaxAmmo;
        ClipCount = MaxClips;
    }

    void Update()
    {
        float AxisVal = Input.GetAxisRaw("Fire1");
        ShootDirection = FpsCamera.transform.forward;

        //Set gun to be automatic or not
        if (GunMode == WeaponMode.Auto)
        {
            if ((Input.GetButton("Fire1") || AxisVal >= 0.5f) && Time.time >= NextShot)
            {
                SpreadDirection();
                NextShot = Time.time + 1f / FireRate;
                Shoot();
            }
        }
        else if (GunMode == WeaponMode.Burst)
        {
            if ((Input.GetButtonDown("Fire1") || (AxisVal >= 0.5f && !hasShot)) && Time.time >= NextShot)
            {
                SpreadDirection();
                NextShot = Time.time + 1f / FireRate;
                if (hasShot && AxisVal == 0)
                    hasShot = false;
                else if (hasShot && AxisVal != 0)
                    return;
                hasShot = true;
                StartCoroutine(BurstShot());
            }
        }
        else
        {
            if ((Input.GetButtonDown("Fire1") || (AxisVal >= 0.5f && !hasShot)) && Time.time >= NextShot)
            {
                NextShot = Time.time + 1f / FireRate;
                if (hasShot && AxisVal == 0)
                    hasShot = false;
                else if (hasShot && AxisVal != 0)
                    return;
                hasShot = true;
                Shoot();
            }
        }
        Debug.DrawLine(FpsCamera.transform.position, ShootDirection * WeaponRange, DebugRayColor);
    }

    void Shoot()
    {
        //Check and play animation
        if (ReticleAnimation != null)
        {
            ReticleAnimation.SetTrigger("Shoot");
            ReticleAnimation.speed = FireRate;
        }

        //Play Audio
        if (GunAudio != null)
            GunAudio.Play();

        //Play Special effects
        if (MuzzleFlash != null)
            MuzzleFlash.Play();

        //Reduce Ammo over time

        //Create shot data and check if weapon has bullet spread
        RaycastHit hit;
        if (Physics.Raycast(FpsCamera.transform.position, ShootDirection, out hit, WeaponRange))
        {
            Debug.Log(hit.transform.name);
            var Health = hit.transform.GetComponent<Health_Guard>();

            //Check if target is valid 
            if (Health != null)
                Health.TakeDamage(GunDamage + 2);
            else if (Health != null)
                Health.TakeDamage(GunDamage);

            //Check if rigidbody component is valid 
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * (HitForce * 100));
            }

            //Check if impact effect is valid
            if (ImpactEffect != null)
            {
                GameObject ImpactGO = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(ImpactGO, ImpactDeathTime);
            }
        }
    }

    IEnumerator BurstShot()
    {
        //Set burst amount
        for (int i = 0; i < BurstAmount; i++)
        {
            //Check and play animation
            if (ReticleAnimation != null)
            {
                ReticleAnimation.SetTrigger("Shoot");
                ReticleAnimation.speed = FireRate * (BurstAmount - 1);
            }

            //Play Audio
            if (GunAudio != null)
                GunAudio.Play();

            //Play Special effects
            if (MuzzleFlash != null)
                MuzzleFlash.Play();

            //Create shot data and check if weapon has bullet spread
            RaycastHit hit;
            if (Physics.Raycast(FpsCamera.transform.position, ShootDirection, out hit, WeaponRange))
            {
                Debug.Log(hit.transform.name + " Damage dealt: " + GunDamage);
                var Health = hit.transform.GetComponent<Health_Guard>();

                //Check if target is valid 
                if (Health != null)
                    Health.TakeDamage(GunDamage + 2);
                else if (Health != null)
                    Health.TakeDamage(GunDamage);

                //Check if rigidbody component is valid 
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * (HitForce * 100));
                }

                //Check if impact effect is valid
                if (ImpactEffect != null)
                {
                    GameObject ImpactGO = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(ImpactGO, ImpactDeathTime);
                }
            }
            yield return new WaitForSeconds(BurstInterval);
        }
    }

    void SpreadDirection()
    {
        ShootDirection.x += Random.Range(-SpreadFactor, SpreadFactor);
        ShootDirection.y += Random.Range(-SpreadFactor, SpreadFactor);
        ShootDirection.z += Random.Range(-SpreadFactor, SpreadFactor);
    }
}
