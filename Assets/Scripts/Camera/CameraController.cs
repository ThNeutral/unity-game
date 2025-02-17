using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float test_crystalCooldown = 0f;

    [SerializeField]
    private GameObject crystalController;

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
        test_HandleCreateCrystal();
    }
    
    private void test_HandleCreateCrystal()
    {
        if (test_crystalCooldown >= 0.5f && Input.GetButtonUp("Fire1"))
        {
            if (Physics.Raycast(transform.position, _camera.transform.forward, out var hit))
            {
                Instantiate(crystalController, hit.point, Quaternion.identity);
            }
            test_crystalCooldown = 0;
        }
        else
        {
            test_crystalCooldown += Time.deltaTime;
        }
    }
}
