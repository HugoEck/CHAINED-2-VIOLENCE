using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUltimateAbility : MonoBehaviour
{
    
    [SerializeField] private Canvas _chooseChainCanvas;

    bool isPlayerActivating = false;
    private bool isPlayerInZone = false;
    private void Start()
    {
        _chooseChainCanvas.gameObject.SetActive(false);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1")
        {
            isPlayerInZone = false;
            _chooseChainCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Only check for input if the player is in the trigger zone
        if (isPlayerInZone)
        {
            isPlayerActivating = InputManager.Instance.GetInteractInput_P1();

            if (isPlayerActivating)
            {
                _chooseChainCanvas.gameObject.SetActive(true);
            }
        }
    }

    public void FireChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.FireChain;

        UltimateAbilityManager.instance.DeactivateUltimateChains();
    }
    public void ElectricChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.ElectricChain;
        UltimateAbilityManager.instance.DeactivateUltimateChains();
    }
    public void GhostChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.GhostChain;
        UltimateAbilityManager.instance.DeactivateUltimateChains();
    }
    public void LaserChain()
    {
        UltimateAbilityManager.instance._currentUltimateAbility = UltimateAbilityManager.UltimateAbilities.LaserChain;
        UltimateAbilityManager.instance.DeactivateUltimateChains();
    }
}
