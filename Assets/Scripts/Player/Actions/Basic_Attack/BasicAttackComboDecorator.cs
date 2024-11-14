using UnityEngine;

public abstract class BasicAttackComboDecorator : IBasicAttack
{
    protected IBasicAttack _wrappedBasicAttack;

    public BasicAttackComboDecorator(IBasicAttack wrappedBasicAttack)
    {
        _wrappedBasicAttack = wrappedBasicAttack;
    }

    public virtual void Execute()
    {
        _wrappedBasicAttack.Execute();
    }

    protected virtual void SwitchWeapon()
    {
        
    }
}
