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

    protected Vector3 moveDirection;
    protected MonoBehaviour target;
    protected EnemyController enemyController;
    protected LootController lootController;
    // Start is called before the first frame update
    protected void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
        lootController = FindFirstObjectByType<LootController>();
        target = enemyController.GetTarget(this);
    }

    // Update is called once per frame
    protected void Update()
    {
        target = enemyController.GetTarget(this);
        if (!enemyController.IsValidTarget(this, target)) return;

        moveDirection = (target.transform.position - transform.position).normalized;
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
    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
