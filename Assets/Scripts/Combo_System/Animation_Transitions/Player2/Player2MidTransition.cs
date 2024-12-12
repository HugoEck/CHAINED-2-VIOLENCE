using UnityEngine;

public class Player2MidTransition : StateMachineBehaviour
{
    private string _nextAnimationToPlay;
    private string currentSubState;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentSubState = Player2ComboManager.instance.currentPlayer2ComboSubstate;

        if (stateInfo.IsName("Transition2"))
        {
            _nextAnimationToPlay = "Combo3";
            animator.SetInteger("ComboIndex", 3);
        }
        else if (stateInfo.IsName("Transition3"))
        {
            _nextAnimationToPlay = "Combo4";
            animator.SetInteger("ComboIndex", 4);
        }
        else if (stateInfo.IsName("Transition4"))
        {
            _nextAnimationToPlay = "Combo5";
            animator.SetInteger("ComboIndex", 5);
        }
        else if (stateInfo.IsName("Transition5"))
        {
            _nextAnimationToPlay = "Combo6";
            animator.SetInteger("ComboIndex", 6);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player2ComboManager.instance.bIsPlayer2Attacking)
        {
            Player2ComboManager.instance.currentPlayer2ComboInSequence = currentSubState + "." + _nextAnimationToPlay;
            animator.Play(currentSubState + "." + _nextAnimationToPlay);
        }

        if (stateInfo.normalizedTime >= 1.0f)
        {
            Player2ComboManager.instance.bIsPlayer2Attacking = false;
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
