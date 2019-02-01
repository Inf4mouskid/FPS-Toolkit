using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [Header("Prerequsite")]
    public Material InnerBody;
    public Material OuterBody;
    public Light BodyLight;
    [Header("Properties")]
    [Tooltip("Set the layer IDs for each part of the shader")]
    public string[] Layers = new string[3];
    public Color MainColor = Color.white;
    public Color OutlineColor = Color.white;
    public Color LightColor = Color.white;

    // Use this for initialization
    void Update()
    {
        //Set the color of the outer body at runtime
        if (OuterBody != null)
        {
            OuterBody.SetColor(Layers[0], MainColor);
        }

        //Set the color of the inner body at runtime
        if (InnerBody != null)
        {
            InnerBody.SetColor(Layers[1], MainColor);
            InnerBody.SetColor(Layers[2], OutlineColor);
        }

        //Check if light is valid
        if (BodyLight != null) BodyLight.color = LightColor;
    }

}
