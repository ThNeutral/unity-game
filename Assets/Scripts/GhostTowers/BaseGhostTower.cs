using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseTower;

public class BaseGhostTower : MonoBehaviour
{
    public enum MaterialState
    {
        VALID,
        INVALID
    }

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material validTransparentMaterial;
    [SerializeField]
    private Material invalidTransparentMaterial;

    private MaterialState materialState = MaterialState.VALID;
    private MaterialState targetMaterialState = MaterialState.INVALID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleGhostTower(); 
    }
    private void HandleGhostTower()
    {
        if (targetMaterialState != materialState)
        {
            if (targetMaterialState == MaterialState.VALID)
            {
                SwitchMaterial(validTransparentMaterial);
                materialState = MaterialState.VALID;
            }
            else
            {
                SwitchMaterial(invalidTransparentMaterial);
                materialState = MaterialState.INVALID;
            }
        }
    }
    private void SwitchMaterial(Material material)
    {
        var materialsCopy = meshRenderer.materials;
        materialsCopy[0] = material;
        meshRenderer.materials = materialsCopy;
    }
    public void SetTargetMaterial(MaterialState ms)
    {
        targetMaterialState = ms;
    }
}
