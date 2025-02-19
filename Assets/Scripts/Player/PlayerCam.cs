using UnityEngine;

public class PlayerCam : MonoBehaviour
{

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

        if (Input.GetButton("Fire1") && placementCounter >= placementCooldown)
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
    }
}