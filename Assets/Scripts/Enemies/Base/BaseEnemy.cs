using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    protected int health = 1;

    [SerializeField]
    protected float speed = 2;

    [SerializeField]
    protected GameObject experiencePrefab;

    [SerializeField]
    protected float towerAgroRadius = 5;

    [SerializeField]
    protected float playerAgroRadius = 1;

    protected Vector3 moveDirection;

    protected PlayerController playerController;
    protected LootController lootController;
    protected EnemyController enemyController;
    protected TowerController towerController;

    protected List<NavigationPoint> route;

    protected bool isInAgro = false;
    protected MonoBehaviour target;

    public virtual EnemyType Type => EnemyType.BASE;

    // Start is called before the first frame update
    protected void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        lootController = FindFirstObjectByType<LootController>();
        enemyController = FindFirstObjectByType<EnemyController>();
        towerController = FindFirstObjectByType<TowerController>();

        route = enemyController.GetRoute(Type, transform.position);
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleMove();
        HandleAttack();
    }

    protected virtual void HandleMove()
    {
        var newIsInAgro = false;
        var (tower, towerDistance) = towerController.GetClosestTower(transform.position);
        if (towerDistance < towerAgroRadius)
        {
            newIsInAgro = true;
            target = tower;
        }

        var playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
        if (playerDistance < playerAgroRadius)
        {
            newIsInAgro = true;
            target = playerController;
        }

        if (newIsInAgro)
        {
            isInAgro = true;
            moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += speed * Time.deltaTime * moveDirection;
            transform.rotation = Quaternion.LookRotation(moveDirection);
            return;
        }

        if (!newIsInAgro && isInAgro)
        {
            isInAgro = false;
            route = enemyController.GetRoute(Type, transform.position);
            target = null;
        }

        if (route.Count == 0)
        {
            moveDirection = Vector3.zero;
            return;
        }

        var point = route[0];
        while (route.Count > 0 && point.Contains(transform.position))
        {
            route.RemoveAt(0);
            if (route.Count == 0)
            {
                moveDirection = Vector3.zero;
                return;
            }
            point = route[0];
        }

        if (route.Count == 0)
        {
            moveDirection = Vector3.zero;
            return;
        }

        moveDirection = point.DirectionWithLock(transform.position);
        transform.rotation = Quaternion.LookRotation(moveDirection);
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    protected virtual void HandleAttack()
    {

    }

    protected virtual void HandleDeath()
    {
        lootController.InstantiateExperienceBlob(experiencePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public bool ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0) 
        {
            HandleDeath();
            return true;
        }
        return false;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
