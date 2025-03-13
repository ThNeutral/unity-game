using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyTargetIntention
    {
        Default,
        KeepDistance
    }
    [SerializeField]
    protected int health = 1;

    [SerializeField]
    protected float speed = 2;

    [SerializeField]
    protected GameObject experiencePrefab;

    [SerializeField]
    protected EnemyTargetIntention targetIntention;

    [SerializeField]
    protected float playerAgroRange = 10f;

    [SerializeField]
    protected float towerAgroRange = 10f;

    [SerializeField]
    protected float treeAgroRange = 10f;

    [SerializeField]
    protected float towerAvoidanceDistance = 3f;

    protected Vector3 moveDirection;
    protected EnemyTarget target;
    protected EnemyController enemyController;
    protected LootController lootController;

    protected bool isControlled = false;
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleTarget();
        HandleMove();
    }
    protected void Initialize()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
        lootController = FindFirstObjectByType<LootController>();
    }
    protected void HandleTarget()
    {
        if (!isControlled) target = enemyController.GetTarget(this);
    }
    protected void HandleMove()
    {
        if (!enemyController.IsValidTarget(this, target)) return;

        moveDirection = (target.Position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDirection;
    }
    public bool DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0) 
        {
            Destroy(gameObject);
            lootController.InstantiateExperienceBlob(transform.position, Quaternion.identity);
            return true;
        }
        return false;
    }
    public Vector3 GetSpeedDirection()
    {
        return moveDirection;
    }
    public float GetSpeedMagnitude()
    {
        return speed;
    }
    public Vector3 GetSpeed()
    {
        return moveDirection * speed;
    }

    public void SetIsControlled(bool value)
    {
        isControlled = value;
    }

    public void SetTarget(EnemyTarget target)
    {
        if (!isControlled)
        {
            Debug.LogError("Cannot set target to not controlled enemy");
            return;
        }
        this.target = target;
    }

    public EnemyTargetIntention GetTargetIntention()
    {
        return targetIntention;
    }

    public float GetPlayerAgroRange()
    {
        return playerAgroRange;
    }

    public float GetTowerAgroRange()
    {
        return towerAgroRange;
    }

    public float GetTreeAgroRange()
    {
        return treeAgroRange;
    }

    public float GetTowerAvoidanceDistance()
    {
        return towerAvoidanceDistance;
    }
}
