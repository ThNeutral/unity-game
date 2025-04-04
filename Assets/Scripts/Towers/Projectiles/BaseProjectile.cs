using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private Vector3 direction;
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private float speed;
    public void SetSpeed(float s)
    {
        speed = s;
    }

    private int damage;
    public void SetDamage(int d)
    {
        damage = d;
    }

    private float maximumRange;
    public void SetMaximumRange(float range)
    {
        maximumRange = range;
    }

    private Vector3 startingPosition;
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = FindFirstObjectByType<EnemyController>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
        if (Vector3.Distance(startingPosition, transform.position) > maximumRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemyController.DealDamageTo(enemy, damage);
            Destroy(gameObject);
        }
    }
}
