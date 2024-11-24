using UnityEngine;

public class Player2StartCombo : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player2ComboManager.instance.bIsPlayer2Attacking = false;
        animator.SetBool("ComboCancelled", false);
        animator.SetBool("ComboOver", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(0) && Player2ComboManager.instance.bIsPlayer2Attacking && animator.GetBool("ComboOver"))
        {
            bool hasComboStarted = false;
            switch (Player2ComboManager.instance.currentEquippedPlayer2WeaponType)
            {
                case Weapon.WeaponType.Unarmed:

                    if(animator.GetInteger("PlayerClass") == 0) // Default
                    {
                        hasComboStarted = true;
                        Player2ComboManager.instance.currentPlayer2ComboInSequence = ComboAnimationStatesData.unarmedSubStateDefault + "." + ComboAnimationStatesData.combosInUnarmedDefaultState[0];
                        animator.Play(Player2ComboManager.instance.currentPlayer2ComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 1) // Tank
                    {
                        hasComboStarted = true;
                        Player2ComboManager.instance.currentPlayer2ComboInSequence = ComboAnimationStatesData.unarmedSubStateTank + "." + ComboAnimationStatesData.combosInUnarmedTankState[0];
                        animator.Play(Player2ComboManager.instance.currentPlayer2ComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 4) // Ranged
                    {
                        //hasComboStarted = true;
                        //Player2ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Ranged.UnarmedCombo1";
                        //animator.Play(/*"UnarmedCombo1"*/Player2ComboManager.instance.currentComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 2) // Warrior
                    {
                        hasComboStarted = true;
                        Player2ComboManager.instance.currentPlayer2ComboInSequence = ComboAnimationStatesData.unarmedSubStateWarrior + "." + ComboAnimationStatesData.combosInUnarmedWarriorState[0];
                        animator.Play(Player2ComboManager.instance.currentPlayer2ComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 3) // Support
                    {
                        hasComboStarted = true;
                        Player2ComboManager.instance.currentPlayer2ComboInSequence = ComboAnimationStatesData.unarmedSubStateSupport + "." + ComboAnimationStatesData.combosInUnarmedSupportState[0];
                        animator.Play(Player2ComboManager.instance.currentPlayer2ComboInSequence);
                    }
                    break;

                case Weapon.WeaponType.TwoHanded:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInTwoHandedState[0]);
                    break;

                case Weapon.WeaponType.OneHanded:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInOneHandedState[0]);
                    break;

                case Weapon.WeaponType.ReallyBigTwoHanded:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInReallyBigTwoHandedState[0]);
                    break;

                case Weapon.WeaponType.Polearm:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInPolearmState[0]);
                    break;

                case Weapon.WeaponType.Dagger:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInDaggerState[0]);
                    break;

                case Weapon.WeaponType.BigPen:
                    hasComboStarted = true;
                    animator.Play(ComboAnimationStatesData.combosInBigPenState[0]);
                    break;
            }

            if (hasComboStarted)
            {
                animator.SetBool("ComboOver", false);
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

