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
    protected int damage = 1;

    [SerializeField]
    protected Rigidbody rb;

    protected Vector3 moveDirection;
    protected BaseTower target;
    protected EnemyController enemyController;
    protected TowerController towerController;
    // Start is called before the first frame update
    private void Start()
    {
        target = enemyController.GetTarget(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!enemyController.IsValidTarget(this, target)) target = enemyController.GetTarget(this);
        if (target == null) return;

        moveDirection = (target.transform.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDirection;
    }
    public virtual bool RecieveDamage(int damage)
    {
        health -= damage;
        if (health <= 0) 
        {
            Destroy(gameObject);
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
    public void SetTowerController(TowerController controller)
    {
        towerController = controller;
    }
}
