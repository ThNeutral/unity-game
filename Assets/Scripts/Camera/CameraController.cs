using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float spawnTowerCooldown = 0f;
    private float spawnSpawnerCooldown = 0f;

    private Camera _camera;
    private TowerController _towerController;
    private EnemyController _enemyController;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _towerController = FindObjectOfType<TowerController>();
        _enemyController = FindObjectOfType<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCreateSpawner();
        HandleCreateTower();
    }

    private void HandleCreateTower()
    {
        if ((Input.GetAxis("Fire1") == 1) && (spawnTowerCooldown > 1f))
        {
            var origin = transform.position;
            var direction = transform.forward;
            if (Physics.Raycast(origin, direction, out var hitInfo))
            {
                spawnTowerCooldown = 0;
                _towerController.PlaceTower(hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward));
            };
        }
        else
        {
            spawnTowerCooldown += Time.deltaTime;
        }
    }

    private void HandleCreateSpawner()
    {
        if ((Input.GetAxis("Fire2") == 1) && (spawnSpawnerCooldown > 1f))
        {
            var origin = transform.position;
            var direction = transform.forward;
            if (Physics.Raycast(origin, direction, out var hitInfo))
            {
                spawnSpawnerCooldown = 0;
                _enemyController.PlaceSpawner(hitInfo.point, Quaternion.LookRotation(hitInfo.transform.forward));
            };
        }
        else
        {
            spawnSpawnerCooldown += Time.deltaTime;
        }
    }
}
