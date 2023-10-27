using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public MeshRenderer backgroundMesh;

    private void Start()
    {
        Invoke("ChangeParallax", 3f);
    }

    public Texture texture
    {
        set
        {
            backgroundMesh.material.mainTexture = value;
        }
    }
    
    public void ChangeParallax()
    {
        if (Loader.instance.readyToUse)
        {
            Debug.Log("TOWNHALL ADDRESSABLE BG");
            texture =  Loader.instance.townhallEnvironment;
        }
    }
}
