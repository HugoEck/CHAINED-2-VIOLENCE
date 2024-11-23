using UnityEngine;

public class Player1StartCombo : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player1ComboManager.instance.player1Animator.SetBool("ComboCancelled", false);
        Player1ComboManager.instance.player1Animator.SetBool("ComboOver", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(0) && Player1ComboManager.instance.bIsPlayer1Attacking && Player1ComboManager.instance.player1Animator.GetBool("ComboOver"))
        {
            bool hasComboStarted = false;
            switch (Player1ComboManager.instance.currentEquippedPlayer1WeaponType)
            {
                case Weapon.WeaponType.Unarmed:

                    if (animator.GetInteger("PlayerClass") == 0) // Default
                    {
                        hasComboStarted = true;
                        Player1ComboManager.instance.currentPlayer1ComboInSequence = ComboAnimationStatesData.unarmedSubStateDefault + "." + ComboAnimationStatesData.combosInUnarmedState[0];
                        animator.Play(Player1ComboManager.instance.currentPlayer1ComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 1) // Tank
                    {
                        //hasComboStarted = true;
                        //Player1ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Tank.UnarmedCombo1";
                        //animator.Play(/*"UnarmedCombo1"*/Player1ComboManager.instance.currentComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 4) // Ranged
                    {
                        //hasComboStarted = true;
                        //Player1ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Ranged.UnarmedCombo1";
                        //animator.Play(/*"UnarmedCombo1"*/Player1ComboManager.instance.currentComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 2) // Warrior
                    {
                        //hasComboStarted = true;
                        //Player1ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Warrior.UnarmedCombo1";
                        //animator.Play(/*"UnarmedCombo1"*/Player1ComboManager.instance.currentComboInSequence);
                    }
                    else if (animator.GetInteger("PlayerClass") == 3) // Support
                    {
                        //hasComboStarted = true;
                        //Player1ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Support.UnarmedCombo1";
                        //animator.Play(/*"UnarmedCombo1"*/Player1ComboManager.instance.currentComboInSequence);
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
                Player1ComboManager.instance.player1Animator.SetBool("ComboOver", false);
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

