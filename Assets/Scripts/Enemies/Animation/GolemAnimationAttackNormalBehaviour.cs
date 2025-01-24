using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GolemAnimationAttackNormalBehaviour : StateMachineBehaviour
{
    private enum AttackStates
    {
        NOT_STARTED,
        HAPPENING,
        FINISHED
    }
    private float length;
    private bool isInAnimation;
    private AttackStates attackState;
    private float counter;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        counter = 0;
        length = stateInfo.length;
        isInAnimation = true;
        attackState = AttackStates.NOT_STARTED;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isInAnimation = false;
        animator.GetComponentInChildren<GolemBehaviour>().SetIsInSpecialAttack(false);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isInAnimation)
        {
            counter += Time.deltaTime;
            if (attackState == AttackStates.NOT_STARTED)
            {
                if (counter > length * 0.6)
                {
                    animator.GetComponentInChildren<GolemBehaviour>().SetIsInSpecialAttack(true);
                    attackState = AttackStates.HAPPENING;
                    counter = 0;
                }
            }
            else if (attackState == AttackStates.HAPPENING)
            {
                if (counter > length * 0.2)
                {
                    animator.GetComponentInChildren<GolemBehaviour>().SetIsInSpecialAttack(true);
                    attackState = AttackStates.FINISHED;
                }
            }
        }
    }
}
