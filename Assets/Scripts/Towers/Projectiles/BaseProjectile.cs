using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected Vector3 direction;
    protected float speed;
    protected int damage;
    protected EnemyController enemyController;
    protected float maximumRange;

    protected Vector3 startingPosition;
    // Start is called before the first frame update
    protected void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
        if (Vector3.Distance(startingPosition, transform.position) > maximumRange)
        {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseProjectile>(out var _)) return;

        if (other.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemyController.DealDamageTo(enemy, damage);
        }
        Destroy(gameObject);
    }
    public void SetSpeed(float s)
    {
        speed = s;
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    public void SetDamage(int d)
    {
        damage = d;
    }
    public void SetEnemyController(EnemyController controller)
    {
        enemyController = controller;
    }
    public void SetMaximumRange(float range)
    {
        maximumRange = range;
    }
}
