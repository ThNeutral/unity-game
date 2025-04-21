using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Summoner : BaseEnemy
{
    private static readonly string DEATH_ANIMATION_NAME = "Armature.Summoner|Death";
    private static readonly string DEATH_TRIGGER_NAME = "Death";
    private static readonly string WALK_ANIMATION_NAME = "Armature.Summoner|Walk";
    private static readonly string WALK_TRIGGER_NAME = "Walk";
    private static readonly string SUMMON_ANIMATION_NAME = "Armature.Summoner|Summon";
    private static readonly string SUMMON_TRIGGER_NAME = "Summon";
    private static readonly string IDLE_ANIMATION_NAME = "Armature.Summoner|Idle";
    private static readonly string IDLE_TRIGGER_NAME = "Idle";

    private enum State
    {
        Idle,
        Walk
    }

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SummonerSpawner behaviour;

    [SerializeField]
    private float summonDelay = 0.25f;
    private float summonCounter = 0;

    private State prevState = State.Walk;
    private State targetState = State.Walk;

    private float deathAnimationLength;
    private float summonAnimationLength;

    private bool isDead = false;
    private bool isInSummonAnimation = false;

    public override EnemyType Type => EnemyType.Summoner;

    private new void Start()
    {
        base.Start();
        enemyController.Register(behaviour);
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == DEATH_ANIMATION_NAME)
            {
                deathAnimationLength = clip.length;
            }
            if (clip.name == SUMMON_ANIMATION_NAME)
            {
                summonAnimationLength = clip.length;
            }
        }
    }

    protected override bool HandleUniqueMove()
    {
        if (isDead)
        {
            moveDirection = Vector3.zero;
            return true;
        }

        var distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);
        if (distanceToPlayer < playerInteractionRadius)
        {
            target = playerController;
            moveDirection = Vector3.zero;
            targetState = State.Idle;
            return true;
        }

        var (tower, distanceToTower) = towerController.GetClosestTower(transform.position);
        if (distanceToTower < towerInteractionRadius)
        {
            target = tower;
            moveDirection = Vector3.zero;
            targetState = State.Idle;
            return true;
        }

        target = null;
        targetState = State.Walk;
        return false;
    }

    protected override void HandleAсtion()
    {
        if (isDead) return;

        if (targetState == State.Idle)
        {
            behaviour.SetTarget(target);
            summonDelay += Time.deltaTime;
            if (!isInSummonAnimation && summonDelay >= summonCounter)
            {
                summonDelay = 0;
                int numOfSummons = behaviour.RequestSummon();
                if (numOfSummons == 0) return;
                animator.SetTrigger(SUMMON_TRIGGER_NAME);
                isInSummonAnimation = true;
                Delay(() => isInSummonAnimation = false, summonAnimationLength);
            }
            if (prevState != State.Idle)
            {
                if (!isInSummonAnimation)
                {
                    animator.SetTrigger(IDLE_TRIGGER_NAME);
                }
                prevState = State.Idle;
            }
        }
        else if (targetState == State.Walk)
        {
            if (prevState != State.Walk)
            {
                animator.SetTrigger(WALK_TRIGGER_NAME);
                prevState = State.Walk;
                summonDelay = 0;
            }
            if (route.Count != 0)
            {
                behaviour.SetPoint(route[0]);
            }
            else
            {
                behaviour.Freeze();
            }
        }
    }
    protected override void HandleDeath()
    {
        isDead = true;
        animator.SetTrigger(DEATH_TRIGGER_NAME);
        Delay(base.HandleDeath, deathAnimationLength);
    }
}
