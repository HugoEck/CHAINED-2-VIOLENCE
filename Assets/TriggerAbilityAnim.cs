using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAbilityAnim : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("UseAbility"))
        {
            animator.SetLayerWeight(layerIndex, 1);

            if(animator.GetInteger("currentPlayerClass") == 1)
            {
                animator.Play("Tank_Ability", layerIndex);
            }
            else if(animator.GetInteger("currentPlayerClass") == 2)
            {
                animator.Play("Warrior_Ability", layerIndex);
            }
            else if (animator.GetInteger("currentPlayerClass") == 3)
            {
                animator.Play("Support_Ability", layerIndex);
            }
            else if (animator.GetInteger("currentPlayerClass") == 4)
            {
                animator.Play("Ranged_Ability", layerIndex);
            }

            animator.SetBool("UseAbility", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
