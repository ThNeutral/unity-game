using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected float projectileSpeed = 10;

    [SerializeField]
    protected float shootSpeed = 0.02f;

    [SerializeField]
    protected int damage = 1;

    [SerializeField]
    protected float maximumRange = 30f;

    [SerializeField]
    protected float dispersionAngle = 10f;

    [SerializeField]
    protected int health = 1;

    protected float counter;

    protected EnemyController enemyController;
    protected TowerController towerController;
    // Start is called before the first frame update
    private void Start()
    {
        
    }
    private void Update()
    {
        counter += Time.deltaTime;
        while (counter > shootSpeed)
        {
            var target = towerController.GetTarget(this);
            if (target != null)
            {
                ShootAt(target);
                counter -= shootSpeed;
            } else
            {
                counter = 0;
            }
        }
    }
    public void ShootAt(BaseEnemy enemyBehaviour)
    {
        var enemyMoveDirection = enemyBehaviour.GetMoveDirection();
        var enemySpeed = enemyBehaviour.GetSpeed();
        var enemyPosition = enemyBehaviour.transform.position;
        var selfPosition = transform.position;

        var estimatedFlightTime = (enemyPosition - selfPosition).magnitude / projectileSpeed;

        var enemyFuturePosition = enemyPosition + enemySpeed * estimatedFlightTime * enemyMoveDirection;

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
    public bool RecieveDamage(int damage)
    {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
            return true;
        }
        
        return false;
    }
    public void SetEnemyController(EnemyController controller)
    {
        enemyController = controller;
    }
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
}
