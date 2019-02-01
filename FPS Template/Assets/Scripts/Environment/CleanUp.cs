using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUp : MonoBehaviour
{
    [Tooltip("Set the time in seconds for how long it should take for an object to despawn")]
    [Range(1, 100)] public int Time = 1;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, Time);
    }
}
