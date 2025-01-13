using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    private int health = 1;

    [SerializeField]
    private float speed = 2;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        do
        {
            moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        } while (moveDirection.magnitude == 0);
        moveDirection = moveDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    public bool DealDamage(int damage)
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
}
