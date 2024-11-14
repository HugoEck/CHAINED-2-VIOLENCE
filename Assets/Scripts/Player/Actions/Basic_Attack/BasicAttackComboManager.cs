using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BasicAttackComboManager : MonoBehaviour
{
    private int _comboStage = 0;
    private List<IBasicAttack> _comboAttacks; // List to hold combo attack stages
    private float comboResetTime = 2f;      // Time in seconds before combo resets
    private float comboOverTimer = 3f;
    private Coroutine comboResetCoroutine;    // Coroutine to track the reset timer

    private bool bCanCombo = true;

    private void Start()
    {
        // Initialize the list of combo stages, starting with the basic attack
        _comboAttacks = new List<IBasicAttack>
        {
            new BasicAttackComboOne(new BasicAttack(transform.position)), // Combo stage 1
            new BasicAttackComboTwo(new BasicAttack(transform.position)), // Combo stage 2
            new BasicAttackComboThree(new BasicAttack(transform.position)), // Combo stage 3
        };
    }

    public void PerformComboAttack()
    {
        if (!bCanCombo) return;

        // Set current attack based on the current combo stage
        var currentAttack = _comboAttacks[_comboStage];

        // Start or restart the timer coroutine to reset the combo if no further input is received
        if (comboResetCoroutine != null)
        {
            StopCoroutine(comboResetCoroutine);
        }
        
        // Execute the current attack
        currentAttack.Execute();
        Debug.Log("Combo attack: " + _comboAttacks[_comboStage]);

        // Move to the next combo stage, cycling back to the start if needed
        _comboStage = (_comboStage + 1) % _comboAttacks.Count;

        if (_comboStage == 0)
        {
            bCanCombo = false;
            StartCoroutine(ComboOverTimer());
        }

        if(bCanCombo)
        {
            comboResetCoroutine = StartCoroutine(ComboResetTimer());
        }       
    }

    private IEnumerator ComboResetTimer()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(comboResetTime);

        // Reset the combo stage
        _comboStage = 0;
        Debug.Log("Combo reset due to timeout.");
    }
    private IEnumerator ComboOverTimer()
    {

        yield return new WaitForSeconds(comboOverTimer);

        bCanCombo = true;
    }
}

