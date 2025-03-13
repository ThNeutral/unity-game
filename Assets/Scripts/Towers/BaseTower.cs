using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseTower : MonoBehaviour
{ 
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float projectileSpeed = 10;

    [SerializeField]
    private float shootSpeed = 0.02f;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float maximumRange = 30f;

    [SerializeField]
    private float dispersionAngle = 10f;

    [SerializeField]
    private float shootRadius = 10f;

    private float counter;

    private LootController lootController;
    private EnemyController enemyController;
    private TowerController towerController;

    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
    }
    void Update()
    {
        HandleLivingTower();
    }
    private void HandleLivingTower()
    {
        counter += Time.deltaTime;
        while (counter > shootSpeed)
        {
            var target = towerController.GetTarget(this);
            if (target != null)
            {
                ShootAt(target);
                counter -= shootSpeed;
            }
            else
            {
                counter = 0;
            }
        }
    }
    public void SetEnemyController(EnemyController controller) 
    {
        enemyController = controller;
    }
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
    public float GetShootRadius()
    {
        return shootRadius;
    }
    public void ShootAt(TowerTarget target)
    {
        var selfPosition = transform.position;

        var estimatedFlightTime = (target.Position - selfPosition).magnitude / projectileSpeed;

        var enemyFuturePosition = target.Position + target.Speed * estimatedFlightTime;

        var xRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);
        var yRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);
        var zRotation = Random.Range(-dispersionAngle / 2, dispersionAngle / 2);

        var direction = Quaternion.Euler(xRotation, yRotation, zRotation) * (enemyFuturePosition - selfPosition).normalized;
        var position = transform.position + direction * 0.75f;
        var rotation = Quaternion.LookRotation(direction);

        var projectileBehaviour = Instantiate(projectile, position, rotation).GetComponent<BaseProjectile>();

        projectileBehaviour.SetDirection(direction);
        projectileBehaviour.SetSpeed(projectileSpeed);
        projectileBehaviour.SetDamage(damage);
        projectileBehaviour.SetEnemyController(enemyController);
        projectileBehaviour.SetMaximumRange(maximumRange);
    }
}
