using UnityEngine;

public class AssignComboAttributesPlayer1 : StateMachineBehaviour
{
    private ComboAttackSO _comboAttackScriptableObject;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



        #region Unarmed

        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedDefaultState, ComboAnimationStatesData.unarmedSubStateDefault);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedTankState, ComboAnimationStatesData.unarmedSubStateTank);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedWarriorState, ComboAnimationStatesData.unarmedSubStateWarrior);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedRangedState, ComboAnimationStatesData.unarmedSubStateRanged);
        ApplyUnarmedCombos(ComboAnimationStatesData.combosInUnarmedSupportState, ComboAnimationStatesData.unarmedSubStateSupport);



        #endregion

        #region Weapons

        ApplyWeaponCombos(ComboAnimationStatesData.combosInTwoHandedState, ComboAnimationStatesData.twoHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInOneHandedState, ComboAnimationStatesData.oneHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInReallyBigTwoHandedState, ComboAnimationStatesData.reallyBigTwoHandedSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInDaggerState, ComboAnimationStatesData.daggerSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInPolearmState, ComboAnimationStatesData.polearmSubState);
        ApplyWeaponCombos(ComboAnimationStatesData.combosInBigPenState, ComboAnimationStatesData.bigPenSubState);



        #endregion

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("DealDamage") && _comboAttackScriptableObject != null)
        {
            Player1ComboManager.instance.DealDamageToEnemies(_comboAttackScriptableObject.attackRange,
                _comboAttackScriptableObject.damage, _comboAttackScriptableObject.stunDuration, _comboAttackScriptableObject.knockback, _comboAttackScriptableObject.maxAngle);
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
        string currentComboInSequence = Player1ComboManager.instance.currentPlayer1ComboInSequence;

        if (Player1ComboManager.instance.currentPlayer1ComboSubstate == weaponSubStateToCheck)
        {
            for (int i = 0; i < weaponSpecificComboSubState.Length; i++)
            {
                if (Player1ComboManager.instance.currentPlayer1Weapon.combos.Length == weaponSpecificComboSubState.Length)
                {
                    if (CheckCurrentComboInSequence(currentComboInSequence, weaponSubStateToCheck, weaponSpecificComboSubState[i]))
                    {
                        _comboAttackScriptableObject = Player1ComboManager.instance.currentPlayer1Weapon.combos[i];
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
        string currentComboInSequence = Player1ComboManager.instance.currentPlayer1ComboInSequence;

        if (Player1ComboManager.instance.currentPlayer1ComboSubstate == unarmedSubStateToCheck)
        {
            for (int i = 0; i < classSpecificCombosSubState.Length; i++)
            {
                if (Player1ComboManager.instance.player1UnarmedCombos.Length == classSpecificCombosSubState.Length)
                {
                    if (CheckCurrentComboInSequence(currentComboInSequence, unarmedSubStateToCheck, classSpecificCombosSubState[i]))
                    {
                        _comboAttackScriptableObject = Player1ComboManager.instance.player1UnarmedCombos[i];
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
