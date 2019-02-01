using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Tooltip("Asset to replace original game object")]
    public GameObject DestroyedAsset;

    //Swaps models to appear destroyed in game
    public void DestroyModel()
    {
        if (DestroyedAsset != null)
            Instantiate(DestroyedAsset, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
