using UnityEngine;

public class Player1FirstTransition : StateMachineBehaviour
{
    private string currentSubState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentSubState = Player1ComboManager.instance.currentPlayer1ComboSubstate;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Player1ComboManager.instance.bIsPlayer1Attacking)
        {
            Player1ComboManager.instance.currentPlayer1ComboInSequence = currentSubState + ".Combo2";
            animator.Play(currentSubState + ".Combo2");

            animator.SetInteger("ComboIndex", 2);
        }

        if (stateInfo.normalizedTime >= 1.0f)
        {
            Player1ComboManager.instance.bIsPlayer1Attacking = false;
            animator.SetBool("ComboCancelled", true);

            // Gradually reduce the layer weight
            float currentWeight = animator.GetLayerWeight(layerIndex);
            float targetWeight = 0f; // Target is to completely blend out
            float blendSpeed = animator.GetFloat("AttackSpeed"); // Adjust this to control the speed of blending

            // Smoothly reduce layer weight
            currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, blendSpeed * Time.deltaTime);
            animator.SetLayerWeight(layerIndex, currentWeight);

            // Check if fully blended to zero and finalize
            if (currentWeight <= 0.01f)
            {
                animator.SetLayerWeight(layerIndex, 0f); // Ensure it ends at exactly zero
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Player1ComboManager.instance.bIsPlayer1Attacking = false;

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
