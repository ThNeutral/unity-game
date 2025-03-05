using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StafProjectile : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private bool allowInvoke = true;

    private EnemyController enemyController;
    
    private void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();    
    }
    private void Update()
    {
        if (allowInvoke)
        {
            Invoke(nameof(BulletDeath), 2f);
            allowInvoke = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemyController.DealDamageTo(enemy, damage);
            BulletDeath();
        }
        else
        {
            Invoke(nameof(BulletDeath), 0.05f);
        }
    }

    private void BulletDeath()
    {
        Destroy(gameObject);
    }
}
