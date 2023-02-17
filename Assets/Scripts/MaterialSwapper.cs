using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Renderer renderer;
    public Material newMaterial;

    private Material originalMaterial;

    private void OnEnable()
    {
        originalMaterial = renderer.material;
        renderer.material = newMaterial;
    }

    private void OnDisable()
    {
        renderer.material = originalMaterial;
    }
}
