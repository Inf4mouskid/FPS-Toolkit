using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    public int pelletCount;
    public float spreadAngle;
    public GameObject Pellet;
    public Transform PelletSpawn;
    List<Quaternion> Pellets;

	// Use this for initialization
	void Awake ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
