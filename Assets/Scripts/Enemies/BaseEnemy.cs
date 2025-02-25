using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    private int health = 1;

    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private GameObject experiencePrefab;

    private Vector3 moveDirection;
    private BaseTower target;
    private EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        target = enemyController.GetTarget(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyController.IsValidTarget(this, target)) target = enemyController.GetTarget(this);
        moveDirection = (target.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDirection;
    }
    public bool DealDamage(int damage, BaseTower shooter)
    {
        health -= damage;
        if (health <= 0) 
        {
            Destroy(gameObject);
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
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
    public void SetEnemyController(EnemyController controller)
    {
        enemyController = controller;
    }
}
