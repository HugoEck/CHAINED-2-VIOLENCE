using UnityEngine;

public class AssignComboAttributesPlayer2 : StateMachineBehaviour
{
    //private ComboAttackSO _comboAttackScriptableObject;

    //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    string currentComboInSequence = Player2ComboManager.instance.currentPlayer2ComboInSequence;

    //    #region Unarmed
    //    if (Player2ComboManager.instance.currentPlayer2ComboSubstate == ComboAnimationStatesData.unarmedSubStateDefault)
    //    {
    //        for (int i = 0; i < ComboAnimationStatesData.combosInUnarmedState.Length; i++)
    //        {
    //            if (Player2ComboManager.instance.player2UnarmedCombos.Length == ComboAnimationStatesData.combosInUnarmedState.Length)
    //            {
    //                if (CheckCurrentComboInSequence(currentComboInSequence, ComboAnimationStatesData.unarmedSubStateDefault, ComboAnimationStatesData.combosInUnarmedState[i]))
    //                {
    //                    _comboAttackScriptableObject = Player2ComboManager.instance.player2UnarmedCombos[i];
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning("You need to assign: " + ComboAnimationStatesData.combosInUnarmedState.Length + " in default combos array");
    //            }
    //        }
    //    }


    //    #endregion

    //    #region Two handed weapons

    //    if (Player2ComboManager.instance.currentPlayer2ComboSubstate == ComboAnimationStatesData.twoHandedSubState)
    //    {
    //        for (int i = 0; i < ComboAnimationStatesData.combosInTwoHandedState.Length; i++)
    //        {
    //            if (Player2ComboManager.instance.currentPlayer2Weapon.combos.Length == ComboAnimationStatesData.combosInTwoHandedState.Length)
    //            {
    //                if (CheckCurrentComboInSequence(currentComboInSequence, ComboAnimationStatesData.twoHandedSubState, ComboAnimationStatesData.combosInTwoHandedState[i]))
    //                {
    //                    _comboAttackScriptableObject = Player2ComboManager.instance.currentPlayer2Weapon.combos[i];
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning("You need to assign: " + ComboAnimationStatesData.combosInTwoHandedState.Length + " in the current weapon's combos array");
    //            }
    //        }

    //    }


    //    #endregion

    //}

    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (animator.GetBool("DealDamage"))
    //    {
    //        Player2ComboManager.instance.DealDamageToEnemies(_comboAttackScriptableObject.attackRange, _comboAttackScriptableObject.damage);
    //        animator.SetBool("DealDamage", false);
    //    }
    //}

    //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    ////override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    ////{
    ////    
    ////}

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

    //public bool CheckCurrentComboInSequence(string hashStringToCompare, string subState, string combo)
    //{
    //    if (hashStringToCompare == subState + "." + combo)
    //    {
    //        return true;
    //    }
    //    else
    //        return false;
    //}
}
