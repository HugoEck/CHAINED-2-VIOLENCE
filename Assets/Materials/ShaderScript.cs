using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material.SetFloat("_EnableShader", 1);
    }
}
