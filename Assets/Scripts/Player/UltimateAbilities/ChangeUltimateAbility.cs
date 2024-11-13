using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUltimateAbility : MonoBehaviour
{
    
    [SerializeField] private Canvas _chooseChainCanvas;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player1")
        {
            bool player1Interacting = InputManager.Instance.GetInteractInput_P1();

            if(player1Interacting)
            {
                _chooseChainCanvas.gameObject.SetActive(true);
            }
        }
        else if(other.gameObject.tag == "Player2")
        {
            bool player2Interacting = InputManager.Instance.GetInteractInput_P2();

            if (player2Interacting)
            {
                _chooseChainCanvas.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            _chooseChainCanvas.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Player2")
        {
            _chooseChainCanvas.gameObject.SetActive(false);
        }
    }
    public void FireChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.FireChain;
    }
    public void ElectricChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.ElectricChain;
    }
    public void GhostChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.GhostChain;
    }
    public void LaserChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.LaserChain;
    }
}
