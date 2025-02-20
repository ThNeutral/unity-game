using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: select
public class PlacementHandler : MonoBehaviour
{
    private enum PlacementStates
    {
        NONE,
        GHOST
    }

    private TowerController towerController;

    [SerializeField] 
    private Camera cam;

    [SerializeField]
    private float placementCooldown = 0.5f;
    private float placementCounter = 0;

    [SerializeField]
    private float maxTowerPlacementDistance = 15f;

    private PlacementStates placementState = PlacementStates.NONE;
    // Start is called before the first frame update
    void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (placementState)
        {
            case PlacementStates.NONE:
                {
                    if (Input.GetButton("Fire1"))
                    {
                        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, float.PositiveInfinity))
                        {
                            placementState = PlacementStates.GHOST;
                            towerController.CreateGhostTower(hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward, hitInfo.normal));
                        }
                    }
                    break;
                }
            case PlacementStates.GHOST:
                {
                    placementCounter += Time.deltaTime;
                    var isButtonPressed = Input.GetButton("Fire1");
                    Debug.DrawRay(cam.transform.position, cam.transform.forward * 5, Color.red, 0.2f);
                    var isHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, float.PositiveInfinity);
                    if (!isButtonPressed)
                    {
                        if (isHit)
                        {
                            if (hitInfo.distance >= maxTowerPlacementDistance)
                            {
                                towerController.SetGhostTowerMaterialState(BaseTower.MaterialState.INVALID);
                            }
                            else
                            {
                                towerController.SetGhostTowerMaterialState(BaseTower.MaterialState.VALID);
                            }
                            towerController.MoveGhostTower(hitInfo.point);
                        }
                    }
                    else
                    {
                        if (isHit && placementCounter >= placementCooldown)
                        {
                            placementState = PlacementStates.NONE;
                            towerController.PlaceTower(hitInfo.point, Quaternion.identity);
                            towerController.DestroyGhostTower();
                            placementCounter = 0;
                        }
                    }
                    break;
                }
        }
    }
}
