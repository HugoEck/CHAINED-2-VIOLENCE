using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1FirstTransition : StateMachineBehaviour
{
    //private string currentSubState;

    //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    currentSubState = Player1ComboManager.instance.currentPlayer1ComboSubstate;
    //}

    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    if (Player1ComboManager.instance.bIsPlayer1Attacking)
    //    {
    //        Player1ComboManager.instance.currentPlayer1ComboInSequence = currentSubState + ".Combo2";
    //        Player1ComboManager.instance.player1Animator.Play(currentSubState + ".Combo2");

    //    }

    //    if (stateInfo.normalizedTime >= 1.0f)
    //    {
    //        Player1ComboManager.instance.bIsPlayer1Attacking = false;
    //        Player1ComboManager.instance.player1Animator.SetBool("ComboCancelled", true);
    //    }

    //}

    //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    Player1ComboManager.instance.bIsPlayer1Attacking = false;

    //}

    //// OnStateMove is called right after Animator.OnAnimatorMove()
    ////override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    ////{
    ////    // Implement code that processes and affects root motion
    ////}

    //// OnStateIK is called right after Animator.OnAnimatorIK()
    ////override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    ////{
    ////    // Implement code that sets up animation IK (inverse kinematics)
    ////}
}
