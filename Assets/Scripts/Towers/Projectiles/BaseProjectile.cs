using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private int damage;
    private EnemyController enemyController;
    private BaseTower shotBy;
    private float maximumRange;
    private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
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
            enemyController.DealDamageTo(enemy, damage, shotBy);
            Destroy(gameObject);
        }
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
    public void SetShotBy(BaseTower tower)
    {
        shotBy = tower;
    }
}
