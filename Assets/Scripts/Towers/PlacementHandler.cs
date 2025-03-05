using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHandler : MonoBehaviour
{
    private enum PlacementStates
    {
        NONE,
        GHOST,
        PLACEMENT,
    }

    private TowerController towerController;
    private GhostTowerController ghostTowerController;
    private TowersDataProvider towersDataProvider;

    [SerializeField] 
    private Camera cam;

    [SerializeField]
    private float placementCooldown = 0.2f;
    private float placementCounter = 0;

    [SerializeField]
    private float ghostCooldown = 0.5f;
    private float ghostCounter = 0;

    [SerializeField]
    private float maxTowerPlacementDistance = 5f;

    [SerializeField]
    private LayerMask placementLayerMask;

    private PlacementStates placementState = PlacementStates.NONE;

    private int selectedTower = 0;
    // Start is called before the first frame update
    void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
        ghostTowerController = FindFirstObjectByType<GhostTowerController>();
        towersDataProvider = FindFirstObjectByType<TowersDataProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (placementState)
        {
            case PlacementStates.NONE:
                {
                    placementCounter += Time.deltaTime;
                    if (Input.GetKey(KeyCode.Q) && placementCounter >= placementCooldown)
                    {
                        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, float.PositiveInfinity, placementLayerMask))
                        {
                            placementState = PlacementStates.GHOST;
                            var towerDatas = towersDataProvider.GetTowerDatas()[selectedTower];
                            ghostTowerController.CreateGhostTower(towerDatas.Ghost, hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward, hitInfo.normal));
                            placementCounter = 0;
                        }
                    }
                    break;
                }
            case PlacementStates.GHOST:
                {
                    if (Input.GetKey(KeyCode.Mouse1))
                    {
                        placementState = PlacementStates.NONE;
                        ghostTowerController.DestroyGhostTower();
                        ghostCounter = 0;
                        break;
                    }

                    var scroll = Input.mouseScrollDelta;
                    if (scroll.y != 0)
                    {
                        var towerDatas = towersDataProvider.GetTowerDatas();
                        selectedTower += scroll.y > 0 ? 1 : -1;
                        if (scroll.y > 0 && selectedTower >= towerDatas.Count) selectedTower = 0;
                        if (scroll.y < 0 && selectedTower < 0) selectedTower = towerDatas.Count - 1;

                        ghostCounter = 0;
                        var previousPosition = ghostTowerController.GetGhostTowerPosition();
                        var previousRotation = ghostTowerController.GetGhostTowerRotation();
                        ghostTowerController.DestroyGhostTower();
                        ghostTowerController.CreateGhostTower(towerDatas[selectedTower].Ghost, previousPosition, previousRotation);

                        break;
                    }

                    ghostCounter += Time.deltaTime;
                    var isHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, float.PositiveInfinity, placementLayerMask);
                    if (!isHit) break;

                    var isValidPlacement = hitInfo.distance < maxTowerPlacementDistance;
                    if (Input.GetKey(KeyCode.Q) && isValidPlacement && ghostCounter >= ghostCooldown)
                    {
                        placementState = PlacementStates.NONE;
                        var towerDatas = towersDataProvider.GetTowerDatas()[selectedTower];
                        towerController.PlaceTower(towerDatas.Tower, hitInfo.point, Quaternion.identity);
                        ghostTowerController.DestroyGhostTower();
                        ghostCounter = 0;
                        break;
                    }
                    else if (isValidPlacement)
                    {
                        ghostTowerController.SetGhostTowerMaterialState(BaseGhostTower.MaterialState.VALID);
                    }
                    else
                    {
                        ghostTowerController.SetGhostTowerMaterialState(BaseGhostTower.MaterialState.INVALID);
                    }
                    ghostTowerController.MoveGhostTower(hitInfo.point);
                    break;
                }
            
        }
    }
}
