using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private enum PlacementStates
    {
        NONE,
        GHOST
    }
    public float sens;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private TowerController towerController;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float placementCooldown = 0.5f;
    private float placementCounter = 0f;

    [SerializeField]
    private float maxTowerPlacementDistance = 10f;

    private PlacementStates placementState = PlacementStates.NONE;

    void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        HandlePlacement();
    }
    private void HandlePlacement()
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
                            towerController.CreateGhostTower(hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward));
                        }
                    }
                    break;
                }
            case PlacementStates.GHOST:
                {
                    var isButtonPressed = Input.GetButton("Fire1");
                    if (!isButtonPressed)
                    {
                        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, float.PositiveInfinity))
                        {
                            towerController.MoveGhostTower(hitInfo.point);
                            towerController.RotateGhostTower(Quaternion.LookRotation(hitInfo.transform.forward));
                        }
                    }
                    return;
                    if (isButtonPressed && placementCounter >= placementCooldown)
                    {
                        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hitInfo, maxTowerPlacementDistance))
                        {
                            // TODO: preview
                            // TODO: only on ground
                            // TODO: select
                            towerController.PlaceTower(hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward));
                            placementCounter = 0;
                        }
                    }
                    else
                    {
                        placementCounter += Time.deltaTime;
                    }
                    break;
                }
        }
    }
}