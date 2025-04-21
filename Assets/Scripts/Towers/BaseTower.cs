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
    private int hp = 10;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float maximumRange = 30f;

    [SerializeField]
    private float dispersionAngle = 10f;

    private float counter;

    private TowerController towerController;

    private BaseEnemy target;
    // Start is called before the first frame update
    void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
    }

    void Update()
    {
        HandleAim();
        HandleLivingTower();
    }

    protected virtual void HandleAim()
    {
        target = towerController.GetTarget(this);
    }

    protected virtual void HandleLivingTower()
    {
        counter += Time.deltaTime;
        while (counter > shootSpeed)
        {
            if (IsValidTarget())
            {
                Shoot();
                counter -= shootSpeed;
            }
            else
            {
                counter = 0;
            }
        }
    }

    private bool IsValidTarget()
    {
        return target != null && Vector3.Distance(target.transform.position, transform.position) < maximumRange;
    }

    public bool RecieveDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void Shoot()
    {
        var enemyMoveDirection = target.GetMoveDirection();
        var enemySpeed = target.GetSpeed();
        var enemyPosition = target.transform.position;
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
        projectileBehaviour.SetMaximumRange(maximumRange);
    }
}
