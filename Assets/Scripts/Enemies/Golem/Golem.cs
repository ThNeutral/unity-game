using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : BaseEnemy
{
    private static readonly string DEATH_ANIMATION_NAME = "Armature|Golem_Death";
    private static readonly string DEATH_TRIGGER_NAME = "Death";
    private static readonly string ATTACK_LEFT_ANIMATION_NAME = "Armature|Golem_Attack_Left";
    private static readonly string ATTACK_LEFT_TRIGGER_NAME = "AttackLeft";
    private static readonly string ATTACK_RIGHT_ANIMATION_NAME = "Armature|Golem_Attack_Right";
    private static readonly string ATTACK_RIGHT_TRIGGER_NAME = "AttackRight";
    private static readonly string ATTACK_AREA_ANIMATION_NAME = "Armature|Golem_Area_Attack";
    private static readonly string ATTACK_AREA_TRIGGER_NAME = "AttackArea";

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private int normalAttackDamage = 1;

    [SerializeField]
    private float normalAttackDelay = 4;
    private float normalAttackCounter = 0;

    [SerializeField]
    private float normalAttackDistance = 2;

    private bool wasPreviousNormalAttackLeft = false;

    [SerializeField]
    private int areaAttackDamage = 3;

    [SerializeField]
    private float areaAttackDelay = 10;
    private float areaAttackCounter = 0;

    [SerializeField]
    private float areaAttackRadius = 5;

    private float deathAnimationLength;
    private float attackLeftAnimationLength;
    private float attackRightAnimationLength;
    private float attackAreaAnimationLength;

    private bool isMovementLocked = false;

    private bool isDead = false;

    public override EnemyType Type => EnemyType.Golem;
    private new void Start()
    {
        base.Start();
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == DEATH_ANIMATION_NAME)
            {
                deathAnimationLength = clip.length;
            }
            else if (clip.name == ATTACK_LEFT_ANIMATION_NAME)
            {
                attackLeftAnimationLength = clip.length;
            }
            else if (clip.name == ATTACK_RIGHT_ANIMATION_NAME)
            {
                attackRightAnimationLength = clip.length;
            }
            else if (clip.name == ATTACK_AREA_ANIMATION_NAME)
            {
                attackAreaAnimationLength = clip.length;
            }
        }
    }

    protected override void HandleAсtion()
    {
        if (isDead) return;

        normalAttackCounter += Time.deltaTime;
        areaAttackCounter += Time.deltaTime;

        if (target == null) return;

        var distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        if (areaAttackCounter >= areaAttackDelay && distanceToTarget < areaAttackRadius)
        {
            areaAttackCounter = 0;
            normalAttackCounter = 0;
            PlayAnimation(ATTACK_AREA_TRIGGER_NAME, attackAreaAnimationLength, true);
            Delay(() =>
            {
                var destroyed = enemyController.DealDamageInArea(transform.position, areaAttackRadius, areaAttackDamage);
                if (destroyed.Contains(target)) target = null;
            },
            attackAreaAnimationLength * 0.5f);
        }

        if (normalAttackCounter >= normalAttackDelay && distanceToTarget < normalAttackDistance)
        {
            normalAttackCounter = 0;
            var triggerName = wasPreviousNormalAttackLeft ? ATTACK_RIGHT_TRIGGER_NAME : ATTACK_LEFT_TRIGGER_NAME;
            var animationLength = wasPreviousNormalAttackLeft ? attackRightAnimationLength : attackLeftAnimationLength;
            wasPreviousNormalAttackLeft = !wasPreviousNormalAttackLeft;
            PlayAnimation(triggerName, animationLength, true);
            Delay(() =>
            {
                switch (target)
                {
                    case BaseTower tower:
                        {
                            if (tower.RecieveDamage(normalAttackDamage)) target = null;
                            break;
                        }
                    case PlayerController player:
                        {
                            if (player.RecieveDamage(normalAttackDamage)) target = null;
                            break;
                        }
                }
            },
            animationLength * 0.3f);
        }
    }

    protected override void HandleMove()
    {
        if (isMovementLocked || isDead)
        {
            moveDirection = Vector3.zero;
            return;
        }

        base.HandleMove();
    }

    protected override void HandleDeath()
    {
        isDead = true;
        animator.SetTrigger(DEATH_TRIGGER_NAME);
        Delay(base.HandleDeath, deathAnimationLength);
    }

    private void PlayAnimation(string name, float length, bool shouldLockMovement)
    {
        animator.SetTrigger(name);
        if (shouldLockMovement)
        {
            LockMovement(length);
        }
    }

    private void LockMovement(float unlockDelay)
    {
        isMovementLocked = true;
        Delay(() => isMovementLocked = false, unlockDelay);
    }
}
