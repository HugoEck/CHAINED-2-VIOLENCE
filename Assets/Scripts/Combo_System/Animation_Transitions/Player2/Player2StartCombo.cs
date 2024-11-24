using UnityEngine;

public class Player2StartCombo : StateMachineBehaviour
{

    //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Player2ComboManager.instance.player2Animator.SetBool("ComboCancelled", false);
    //    Player2ComboManager.instance.player2Animator.SetBool("ComboOver", true);
    //}

    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (!animator.IsInTransition(0) && Player2ComboManager.instance.bIsPlayer2Attacking && Player2ComboManager.instance.player2Animator.GetBool("ComboOver"))
    //    {
    //        bool hasComboStarted = false;
    //        switch (Player2ComboManager.instance.currentEquippedPlayer2WeaponType)
    //        {
    //            case Weapon.WeaponType.Unarmed:

    //                if (animator.GetInteger("PlayerClass") == 0) // Default
    //                {
    //                    hasComboStarted = true;
    //                    Player2ComboManager.instance.currentPlayer2ComboInSequence = "Base Layer.Attack Combos.Unarmed Default.UnarmedCombo1";
    //                    animator.Play(/*"UnarmedCombo1"*/Player1ComboManager.instance.currentPlayer1ComboInSequence);
    //                }
    //                else if (animator.GetInteger("PlayerClass") == 1) // Tank
    //                {
    //                    //hasComboStarted = true;
    //                    //Player2ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Tank.UnarmedCombo1";
    //                    //animator.Play(/*"UnarmedCombo1"*/Player2ComboManager.instance.currentComboInSequence);
    //                }
    //                else if (animator.GetInteger("PlayerClass") == 4) // Ranged
    //                {
    //                    //hasComboStarted = true;
    //                    //Player2ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Ranged.UnarmedCombo1";
    //                    //animator.Play(/*"UnarmedCombo1"*/Player2ComboManager.instance.currentComboInSequence);
    //                }
    //                else if (animator.GetInteger("PlayerClass") == 2) // Warrior
    //                {
    //                    //hasComboStarted = true;
    //                    //Player2ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Warrior.UnarmedCombo1";
    //                    //animator.Play(/*"UnarmedCombo1"*/Player2ComboManager.instance.currentComboInSequence);
    //                }
    //                else if (animator.GetInteger("PlayerClass") == 3) // Support
    //                {
    //                    //hasComboStarted = true;
    //                    //Player2ComboManager.instance.currentComboInSequence = "Base Layer.Attack Combos.Unarmed Support.UnarmedCombo1";
    //                    //animator.Play(/*"UnarmedCombo1"*/Player2ComboManager.instance.currentComboInSequence);
    //                }
    //                break;

    //            case Weapon.WeaponType.TwoHanded:
    //                hasComboStarted = true;
    //                Player2ComboManager.instance.currentPlayer2ComboInSequence = "Base Layer.Attack Combos.Two Handed.TwoHandedCombo1";
    //                animator.Play("TwoHandedCombo1");
    //                break;

    //            case Weapon.WeaponType.OneHanded:
    //                hasComboStarted = true;
    //                animator.Play("OneHandedCombo1");
    //                break;
    //        }

    //        if (hasComboStarted)
    //        {
    //            Player2ComboManager.instance.player2Animator.SetBool("ComboOver", false);
    //        }

    //    }
    //}

    //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Player2ComboManager.instance.bIsPlayer2Attacking = false;
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

