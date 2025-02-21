using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: select
public class GhostTowerController : MonoBehaviour
{
    private GameObject ghostTower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateGhostTower(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (ghostTower != null)
        {
            Debug.LogError("Called CreateGhostTower when ghost tower is already instantiated");
            return;
        }
        ghostTower = Instantiate(prefab, position, rotation);
    }
    public void MoveGhostTower(Vector3 position)
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called MoveGhostTower when ghost tower is not instantiated");
            return;
        }
        ghostTower.transform.position = position;
    }
    public void RotateGhostTower(Quaternion rotation)
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called RotateGhostTower when ghost tower is not instantiated");
            return;
        }
        ghostTower.transform.rotation = rotation;
    }
    public void SetGhostTowerMaterialState(BaseGhostTower.MaterialState ms)
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called SetGhostTowerMaterialState when ghost tower is not instantiated");
            return;
        }
        ghostTower.GetComponentInChildren<BaseGhostTower>().SetTargetMaterial(ms);
    }
    public void DestroyGhostTower()
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called DestroyGhostTower when ghost tower is not instantiated");
            return;
        }
        Destroy(ghostTower);
        ghostTower = null;
    }
    public Vector3 GetGhostTowerPosition()
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called GetGhostTowerPosition when ghost tower is not instantiated");
            return new Vector3();
        }
        return ghostTower.transform.position;
    }
    public Quaternion GetGhostTowerRotation()
    {
        if (ghostTower == null)
        {
            Debug.LogError("Called GetGhostTowerRotation when ghost tower is not instantiated");
            return new Quaternion();
        }
        return ghostTower.transform.rotation;
    }
}
