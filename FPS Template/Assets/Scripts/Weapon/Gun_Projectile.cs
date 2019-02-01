using System.Collections;
using UnityEngine;

public class Gun_Projectile : MonoBehaviour
{
    //Structs
    public enum WeaponMode { Auto, Burst, Semi }

    //Script GUI Variables
    [Header("Components")] //Weapon components tag
    public Transform BulletSpawn;
    public GameObject BulletPrefab;
    public ParticleSystem MuzzleFlash;
    public Animator ReticleAnimation;
    [Space]
    [Header("Configuration")] //Weapon configuration tag
    public WeaponMode GunMode;
    [Range(1, 100)] public int MaxAmmo = 100;
    [Range(1, 20)] public int MaxClips = 1;
    [Range(20f, 100f)] public float BulletSpeed = 20f;
    [Range(0, 5)] public int BulletSpread = 1;
    [Range(1, 50)] public int BulletDeathTime = 1;
    [Space]
    [Header("Burst Settings")] //Weapon configuration tag
    [Range(2, 4)] public int BurstAmount = 3;
    [Range(0.03f, 0.1f)] public float BurstInterval = 0.1f;
    [Space]
    [Header("Stats")] // Weapon stats tag
    [Range(1f, 25f)] public float FireRate = 5f;
    [Range(0.01f, 100.0f)] public float GunDamage = 0.5f;
    [Range(10.0f, 200.0f)] public float WeaponRange = 50f;
    [Header("Extras")] // Extras tag
    public Color RayColor = Color.green;

    //Private Variables -- Unity
    private Camera FpsCamera;
    private AudioSource GunAudio;
    private Vector3 ShootDirection;

    //Private Variables -- Language
    private bool hasShot = false;
    private float NextShot = 0;
    private float SpreadFactor;
    private int CurrentAmmo;

    //Get prerequsite data
    void Start()
    {
        GunAudio = GetComponent<AudioSource>();
        FpsCamera = GetComponentInParent<Camera>();
        SpreadFactor = (float)BulletSpread / 100;
    }

    void FixedUpdate()
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
                if (hasShot && AxisVal == 0)
                {
                    hasShot = false;
                }
                SpreadDirection();
                NextShot = Time.time + 1f / FireRate;
                hasShot = true;
                StartCoroutine(BurstShot());
            }
        }
        else
        {
            if ((Input.GetButtonDown("Fire1") || (AxisVal >= 0.5f && !hasShot)) && Time.time >= NextShot)
            {
                NextShot = Time.time + 1f / FireRate;
                Shoot();
            }
        }
        Debug.DrawLine(FpsCamera.transform.position, ShootDirection * WeaponRange, RayColor);
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

        if (BulletPrefab != null && BulletSpawn != null)
        {
            var Bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            Bullet.GetComponent<Rigidbody>().velocity = ShootDirection * BulletSpeed;
            Destroy(Bullet, (float)BulletDeathTime / 100);
        }
    }

    IEnumerator BurstShot()
    {
        //Set burst amount
        for (int i = 0; i < BurstAmount; i++)
        {
            Debug.Log("Bullet in Burst array: " + (i + 1));
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
            if (BulletPrefab != null && BulletSpawn != null)
            {
                var Bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
                Bullet.GetComponent<Rigidbody>().velocity = ShootDirection * BulletSpeed;
                Destroy(Bullet, (float)BulletDeathTime / 100);
            }
            yield return new WaitForSeconds(BurstInterval);
        }
    }

    void SpreadDirection()
    {
        ShootDirection = FpsCamera.transform.forward;
        ShootDirection.x += Random.Range(-SpreadFactor, SpreadFactor);
        ShootDirection.y += Random.Range(-SpreadFactor, SpreadFactor);
        ShootDirection.z += Random.Range(-SpreadFactor, SpreadFactor);
    }
}
