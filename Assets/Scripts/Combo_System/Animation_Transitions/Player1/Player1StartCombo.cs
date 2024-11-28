using UnityEngine;

public class Player1StartCombo : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player1ComboManager.instance.bIsPlayer1Attacking)
        {
            animator.SetBool("IsMoving", false);
            Attack(animator);
        }
        Player1ComboManager.instance.bIsPlayer1Attacking = false;
        animator.SetBool("ComboCancelled", false);
        animator.SetBool("ComboOver", true);
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Attack(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player1ComboManager.instance.bIsPlayer1Attacking)
        {
            animator.SetBool("IsMoving", false);
            Attack(animator);
        }
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

    private void Attack(Animator animator)
    {
        if (!animator.IsInTransition(0) && Player1ComboManager.instance.bIsPlayer1Attacking && animator.GetBool("ComboOver"))
        {
            bool hasComboStarted = false;
            switch (Player1ComboManager.instance.currentEquippedPlayer1WeaponType)
            {
                case Weapon.WeaponType.Unarmed:

                    if (Player1ComboManager.instance.currentPlayer1Class == PlayerCombat.PlayerClass.Default) // Default
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateDefault + "." + ComboAnimationStatesData.combosInUnarmedDefaultState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    else if (Player1ComboManager.instance.currentPlayer1Class == PlayerCombat.PlayerClass.Tank) // Tank
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateTank + "." + ComboAnimationStatesData.combosInUnarmedTankState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    else if (Player1ComboManager.instance.currentPlayer1Class == PlayerCombat.PlayerClass.Ranged) // Ranged
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateRanged + "." + ComboAnimationStatesData.combosInUnarmedRangedState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    else if (Player1ComboManager.instance.currentPlayer1Class == PlayerCombat.PlayerClass.Warrior) // Warrior
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateWarrior + "." + ComboAnimationStatesData.combosInUnarmedWarriorState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    else if (Player1ComboManager.instance.currentPlayer1Class == PlayerCombat.PlayerClass.Support) // Support
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateSupport + "." + ComboAnimationStatesData.combosInUnarmedSupportState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    break;

                case Weapon.WeaponType.TwoHanded:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.twoHandedSubState + "." + ComboAnimationStatesData.combosInTwoHandedState[0];
                    animator.Play(ComboAnimationStatesData.combosInTwoHandedState[0]);
                    break;

                case Weapon.WeaponType.OneHanded:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.oneHandedSubState + "." + ComboAnimationStatesData.combosInOneHandedState[0];
                    animator.Play(ComboAnimationStatesData.combosInOneHandedState[0]);
                    break;

                case Weapon.WeaponType.ReallyBigTwoHanded:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.reallyBigTwoHandedSubState + "." + ComboAnimationStatesData.combosInReallyBigTwoHandedState[0];
                    animator.Play(ComboAnimationStatesData.combosInReallyBigTwoHandedState[0]);
                    break;

                case Weapon.WeaponType.Polearm:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.polearmSubState + "." + ComboAnimationStatesData.combosInPolearmState[0];
                    animator.Play(ComboAnimationStatesData.combosInPolearmState[0]);
                    break;

                case Weapon.WeaponType.Dagger:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.daggerSubState + "." + ComboAnimationStatesData.combosInDaggerState[0];
                    animator.Play(ComboAnimationStatesData.combosInDaggerState[0]);
                    break;

                case Weapon.WeaponType.BigPen:
                    hasComboStarted = true;
                    Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.bigPenSubState + "." + ComboAnimationStatesData.combosInBigPenState[0];
                    animator.Play(ComboAnimationStatesData.combosInBigPenState[0]);
                    break;

            }

            if (hasComboStarted)
            {
                animator.SetBool("ComboOver", false);
                animator.SetInteger("ComboIndex", 1);
            }

        }
    }
}

