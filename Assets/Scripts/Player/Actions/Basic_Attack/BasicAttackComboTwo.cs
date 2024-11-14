
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// This is the second combo in the basic attack 
/// </summary>
public class BasicAttackComboTwo : BasicAttackComboDecorator
{
    public BasicAttackComboTwo(IBasicAttack wrappedBasicAttack) 
        : base(wrappedBasicAttack)
    {
        
    }

    public override void Execute()
    {
        _wrappedBasicAttack.Execute();
    }

    protected override void SwitchWeapon()
    {
        if (_wrappedBasicAttack is BasicAttack basicAttack)
        {
            
        }
    }
}

