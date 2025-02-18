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

    [SerializeField]
    private GameObject crystalController;

    [SerializeField]
    private int specialAttackDamage = 3;
    [SerializeField]
    private int normalAttackDamage = 1;

    private float normalAttackCounter = 0f;
    private float specialAttackCounter = 0f;

    private bool isInAttack;
    private bool isInSpecialAttack;

    private Dictionary<BaseTower, bool> touchedTowersByNormalAttack = new();
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
            if (isInAttack)
            {
                if (touchedTowersByNormalAttack.TryGetValue(behaviour, out var _)) return;
                towerController.DealDamage(behaviour, normalAttackDamage);
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
    public void StartSpecialAttack()
    {
        isInSpecialAttack = true;
        Debug.DrawRay(transform.position + 1.5f * transform.forward + 2 * transform.up, -4 * transform.up, Color.red, float.PositiveInfinity);
        if (Physics.Raycast(transform.position + 1.5f * transform.forward + 2 * transform.up, -4 * transform.up, out var hitInfo))
        {
            Instantiate(crystalController, hitInfo.point + 1 * hitInfo.transform.up, Quaternion.LookRotation(hitInfo.transform.forward));
        }
    }

    public void EndSpecialAttack()
    {
        isInSpecialAttack = false;
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
}
