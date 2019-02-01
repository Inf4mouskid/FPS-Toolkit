using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
     public float RotSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotSpeed, 0, 0);
    }
}
