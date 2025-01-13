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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseEnemy>(out var _))
        {
            enemyController.DealDamageTo(other.gameObject, damage);
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
}
