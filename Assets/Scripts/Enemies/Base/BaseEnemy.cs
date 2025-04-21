using System;
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
    protected float towerInteractionRadius = 5;

    [SerializeField]
    protected float playerInteractionRadius = 1;

    protected Vector3 moveDirection;

    protected PlayerController playerController;
    protected LootController lootController;
    protected EnemyController enemyController;
    protected TowerController towerController;

    protected List<NavigationPoint> route;

    protected bool isInAgro = false;
    protected MonoBehaviour target;

    public virtual EnemyType Type => EnemyType.Base;

    // Start is called before the first frame update
    protected void Start()
    {
        HandleInitialize();
        HandleInitialRoute();
    }

    protected virtual void HandleInitialize()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        lootController = FindFirstObjectByType<LootController>();
        enemyController = FindFirstObjectByType<EnemyController>();
        towerController = FindFirstObjectByType<TowerController>();
    }

    protected virtual void HandleInitialRoute()
    {
        route = enemyController.GetRoute(Type, transform.position);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!HandleUniqueMove())
        {
            HandleRoute();
        }
        HandleMove();
        HandleAсtion();
    }
    protected virtual bool HandleUniqueMove()
    {
        var newIsInAgro = false;
        var (tower, towerDistance) = towerController.GetClosestTower(transform.position);
        if (towerDistance < towerInteractionRadius)
        {
            newIsInAgro = true;
            target = tower;
        }

        var playerDistance = Vector3.Distance(playerController.transform.position, transform.position);
        if (playerDistance < playerInteractionRadius)
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
            return true;
        }

        if (!newIsInAgro && isInAgro)
        {
            isInAgro = false;
            route = enemyController.GetRoute(Type, transform.position);
            target = null;
        }

        return false;
    }

    protected virtual void HandleRoute()
    {
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
    }

    protected virtual void HandleMove()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    protected virtual void HandleAсtion()
    {

    }

    protected virtual void HandleDeath()
    {
        lootController.InstantiateExperienceBlob(experiencePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected void Delay(Action func, float delaySeconds)
    {
        StartCoroutine(DelayInternal(func, delaySeconds));
    }

    private IEnumerator DelayInternal(Action func, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        func();
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
