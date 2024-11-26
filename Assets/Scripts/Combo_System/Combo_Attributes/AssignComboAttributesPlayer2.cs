using UnityEngine;

public class AssignComboAttributesPlayer2 : StateMachineBehaviour
{
    private ComboAttackSO _comboAttackScriptableObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        string currentComboInSequence = Player2ComboManager.instance.currentPlayer2ComboInSequence;

        #region Unarmed

        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedDefaultState, ComboAnimationStatesData.unarmedSubStateDefault);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedTankState, ComboAnimationStatesData.unarmedSubStateTank);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedWarriorState, ComboAnimationStatesData.unarmedSubStateWarrior);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedRangedState, ComboAnimationStatesData.unarmedSubStateRanged);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedSupportState, ComboAnimationStatesData.unarmedSubStateSupport);

        //if (Player2ComboManager.instance.currentPlayer2ComboSubstate == ComboAnimationStatesData.unarmedSubStateDefault)
        //{
        //    for (int i = 0; i < ComboAnimationStatesData.combosInUnarmedState.Length; i++)
        //    {
        //        if (Player2ComboManager.instance.player2UnarmedCombos.Length == ComboAnimationStatesData.combosInUnarmedState.Length)
        //        {
        //            if (CheckCurrentComboInSequence(currentComboInSequence, ComboAnimationStatesData.unarmedSubStateDefault, ComboAnimationStatesData.combosInUnarmedState[i]))
        //            {
        //                _comboAttackScriptableObject = Player2ComboManager.instance.player2UnarmedCombos[i];
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogWarning("You need to assign: " + ComboAnimationStatesData.combosInUnarmedState.Length + " in default combos array");
        //        }
        //    }
        //}


        #endregion

        #region Weapons

        ApplyWeaponCombos(ComboAnimationStatesData.combosInTwoHandedState, ComboAnimationStatesData.twoHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInOneHandedState, ComboAnimationStatesData.oneHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInReallyBigTwoHandedState, ComboAnimationStatesData.reallyBigTwoHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInDaggerState, ComboAnimationStatesData.daggerSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInPolearmState, ComboAnimationStatesData.polearmSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInBigPenState, ComboAnimationStatesData.bigPenSubState);

        //if (Player2ComboManager.instance.currentPlayer2ComboSubstate == ComboAnimationStatesData.twoHandedSubState)
        //{
        //    for (int i = 0; i < ComboAnimationStatesData.combosInTwoHandedState.Length; i++)
        //    {
        //        if (Player2ComboManager.instance.currentPlayer2Weapon.combos.Length == ComboAnimationStatesData.combosInTwoHandedState.Length)
        //        {
        //            if (CheckCurrentComboInSequence(currentComboInSequence, ComboAnimationStatesData.twoHandedSubState, ComboAnimationStatesData.combosInTwoHandedState[i]))
        //            {
        //                _comboAttackScriptableObject = Player2ComboManager.instance.currentPlayer2Weapon.combos[i];
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogWarning("You need to assign: " + ComboAnimationStatesData.combosInTwoHandedState.Length + " in the current weapon's combos array");
        //        }
        //    }

        //}


        #endregion

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("DealDamage"))
        {
            Player2ComboManager.instance.DealDamageToEnemies(_comboAttackScriptableObject.attackRange, _comboAttackScriptableObject.damage, _comboAttackScriptableObject.stunDuration, _comboAttackScriptableObject.knockback);
            animator.SetBool("DealDamage", false);
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

    public bool CheckCurrentComboInSequence(string hashStringToCompare, string subState, string combo)
    {
        if (hashStringToCompare == subState + "." + combo)
        {
            return true;
        }
        else
            return false;
    }

    private void ApplyWeaponCombos(string[] weaponSpecificComboSubState, string weaponSubStateToCheck)
    {
        string currentComboInSequence = Player2ComboManager.instance.currentPlayer2ComboInSequence;

        if (Player2ComboManager.instance.currentPlayer2ComboSubstate == weaponSubStateToCheck)
        {
            for (int i = 0; i < weaponSpecificComboSubState.Length; i++)
            {
                if (Player2ComboManager.instance.currentPlayer2Weapon.combos.Length == weaponSpecificComboSubState.Length)
                {
                    if (CheckCurrentComboInSequence(currentComboInSequence, weaponSubStateToCheck, weaponSpecificComboSubState[i]))
                    {
                        _comboAttackScriptableObject = Player2ComboManager.instance.currentPlayer2Weapon.combos[i];
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("You need to assign: " + weaponSpecificComboSubState.Length + " in the current weapon's combos array");
                }
            }

        }
    }
    private void ApplyUnarmedCombos(string[] classSpecificCombosSubState, string unarmedSubStateToCheck)
    {
        string currentComboInSequence = Player2ComboManager.instance.currentPlayer2ComboInSequence;

        if (Player2ComboManager.instance.currentPlayer2ComboSubstate == unarmedSubStateToCheck)
        {
            for (int i = 0; i < classSpecificCombosSubState.Length; i++)
            {
                if (Player2ComboManager.instance.player2UnarmedCombos.Length == classSpecificCombosSubState.Length)
                {
                    if (CheckCurrentComboInSequence(currentComboInSequence, unarmedSubStateToCheck, classSpecificCombosSubState[i]))
                    {
                        _comboAttackScriptableObject = Player2ComboManager.instance.player2UnarmedCombos[i];
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("You need to assign: " + classSpecificCombosSubState.Length + " in default combos array");
                }
            }
        }
    }
}
