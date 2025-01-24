using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GolemBehaviour : BaseEnemy
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float normalAttackDelay = 2f;

    [SerializeField]
    private float specialAttackDelay = 5f;

    private float normalAttackCounter = 0f;
    private float specialAttackCounter = 0f;

    private bool isInAttack;
    private bool isInSpecialAttack;

    private Dictionary<BaseTower, bool> touchedTowersByNormalAttack = new();
    private Dictionary<BaseTower, bool> touchedTowersBySpecialAttack = new();
    // Start is called before the first frame update
    private void Start()
    {
        target = enemyController.GetTarget(this);
    }

    // Update is called once per frame
    private void Update()
    {
        HandleTarget();
        HandleMove();
        HandleAttack();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!isInAttack && !isInSpecialAttack) return;

        var behaviour = collision.gameObject.GetComponentInChildren<BaseTower>();

        if (behaviour != null)
        {
            if (isInSpecialAttack)
            {
                if (touchedTowersBySpecialAttack.TryGetValue(behaviour, out var _)) return;
                towerController.DealDamage(behaviour, 3);
                touchedTowersBySpecialAttack.Add(behaviour, true);

            } else if (isInAttack)
            {
                if (touchedTowersByNormalAttack.TryGetValue(behaviour, out var _)) return;
                towerController.DealDamage(behaviour, 1);
                touchedTowersByNormalAttack.Add(behaviour, true);
            }
        }
    }
    private void HandleTarget()
    {
        if (!enemyController.IsValidTarget(this, target) && !(enemyController.TargetsCount() == 0))
            target = enemyController.GetTarget(this);
    }
    private void HandleMove()
    {
        if (target != null)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.position += speed * Time.deltaTime * moveDirection;
        }
    }

    private void HandleAttack()
    {
        normalAttackCounter += Time.deltaTime;
        specialAttackCounter += Time.deltaTime;

        if (specialAttackCounter > specialAttackDelay)
        {
            if (!target.IsDestroyed() && Vector3.Distance(transform.position, target.transform.position) < 6f)
            {
                animator.SetTrigger("AttackArea");
                specialAttackCounter = 0;
                normalAttackCounter = 0;
            }
        }

        if (normalAttackCounter > normalAttackDelay)
        {
            if (!target.IsDestroyed() && Vector3.Distance(transform.position, target.transform.position) < 4f)
            {
                animator.SetTrigger("AttackLeft");
                normalAttackCounter = 0;
            }
        }
    }

    public override bool RecieveDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            animator.SetTrigger("Death");
            moveDirection = Vector3.zero;
            return true;
        }
        return false;
    }

    public void SetIsInAttack(bool b)
    {
        if (b) touchedTowersByNormalAttack = new();
        isInAttack = b;
    }

    public void SetIsInSpecialAttack(bool b)
    {
        if (b) touchedTowersBySpecialAttack = new();
        isInSpecialAttack = b;
    }
}
