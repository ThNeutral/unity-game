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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetEnemyController(EnemyController controller) {
        enemyController = controller;
    }
    public void AddToCounter(float time)
    {
        counter += time;
    }

    public bool IsReady()
    {
        if (counter > shootSpeed) 
        {
            counter = 0;
            return true;
        }

        return false;
    }
    public void ShootAt(GameObject enemy)
    {
        var enemyBehaviour = enemy.GetComponent<BaseEnemy>();
        ShootAt(enemy, enemyBehaviour);
    }
    public void ShootAt(GameObject enemy, BaseEnemy enemyBehaviour)
    {
        var enemyMoveDirection = enemyBehaviour.GetMoveDirection();
        var enemySpeed = enemyBehaviour.GetSpeed();
        var enemyPosition = enemy.transform.position;
        var selfPosition = transform.position;

        var t = (enemyPosition - selfPosition).magnitude / projectileSpeed;

        var enemyFuturePosition = enemyPosition + enemySpeed * t * enemyMoveDirection;

        var direction = (enemyFuturePosition - selfPosition).normalized;
        var rotation = Quaternion.LookRotation(direction);

        var projectileBehaviour = Instantiate(projectile, transform.position, rotation).GetComponent<BaseProjectile>();

        projectileBehaviour.SetDirection(direction);
        projectileBehaviour.SetSpeed(projectileSpeed);
        projectileBehaviour.SetDamage(damage);
        projectileBehaviour.SetEnemyController(enemyController);
    }
}
