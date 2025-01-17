using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float projectileSpeed = 10;

    [SerializeField]
    private float shootSpeed = 0.1f;

    [SerializeField]
    private int damage = 1;

    private float counter;

    private EnemyController enemyController;
    private TowerController towerController;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > shootSpeed) 
        {
            var target = towerController.GetTarget(this);
            if (target != null)
            {
                ShootAt(target);
                counter = 0;
            }
        }
    }
    public void SetEnemyController(EnemyController controller) {
        enemyController = controller;
    }
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
    public void ShootAt(BaseEnemy enemyBehaviour)
    {
        var enemyMoveDirection = enemyBehaviour.GetMoveDirection();
        var enemySpeed = enemyBehaviour.GetSpeed();
        var enemyPosition = enemyBehaviour.transform.position;
        var selfPosition = transform.position;

        var estimatedFlightTime = (enemyPosition - selfPosition).magnitude / projectileSpeed;

        var enemyFuturePosition = enemyPosition + enemySpeed * estimatedFlightTime * enemyMoveDirection;

        var direction = (enemyFuturePosition - selfPosition).normalized;
        var position = transform.position + direction * 0.5f;
        var rotation = Quaternion.LookRotation(direction);

        var projectileBehaviour = Instantiate(projectile, position, rotation).GetComponent<BaseProjectile>();

        projectileBehaviour.SetDirection(direction);
        projectileBehaviour.SetSpeed(projectileSpeed);
        projectileBehaviour.SetDamage(damage);
        projectileBehaviour.SetEnemyController(enemyController);
    }
}
