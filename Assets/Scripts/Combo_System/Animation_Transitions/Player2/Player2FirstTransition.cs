using UnityEngine;

public class Player2FirstTransition : StateMachineBehaviour
{
    private string currentSubState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentSubState = Player2ComboManager.instance.currentPlayer2ComboSubstate;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Player2ComboManager.instance.bIsPlayer2Attacking)
        {
            Player2ComboManager.instance.currentPlayer2ComboInSequence = currentSubState + ".Combo2";
            animator.Play(currentSubState + ".Combo2");

        }

        if (stateInfo.normalizedTime >= 1.0f)
        {
            Player2ComboManager.instance.bIsPlayer2Attacking = false;
            animator.SetBool("ComboCancelled", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Player2ComboManager.instance.bIsPlayer2Attacking = false;

    }

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
