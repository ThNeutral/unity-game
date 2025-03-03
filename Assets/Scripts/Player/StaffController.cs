using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour
{
    [SerializeField] private float sens;
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Camera Cam;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootForce, timeBetweenShooting;
    private bool readyToShoot, allowInvoke;

    void Awake()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);



        if (Input.GetKey(KeyCode.Mouse0) && readyToShoot)
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        readyToShoot = false;
        Ray ray = Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //рэйкаст на центр экрана
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //далеко от игрока

        Vector3 direction = targetPoint - shootPoint.position;

        GameObject currentProjectile = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        currentProjectile.transform.forward = direction.normalized;

        currentProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        if (allowInvoke)
        {
            Invoke(nameof(ResetShoot), timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
