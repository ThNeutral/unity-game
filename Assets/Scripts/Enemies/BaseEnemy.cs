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
    private LootController lootController;
    private EnemyController enemyController;

    private List<Vector3> route;

    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
        enemyController = FindFirstObjectByType<EnemyController>();

        route = enemyController.GetRoute(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (route.Count == 0) return;
        if (Vector3.Distance(transform.position, route[0]) < 0.5f)
        {
            route.RemoveAt(0);
            if (route.Count == 0) return;
        }
        moveDirection = (route[0] - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    public bool ReceiveDamage(int damage)
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
