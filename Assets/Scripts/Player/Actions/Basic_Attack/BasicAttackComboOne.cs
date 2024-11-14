
/// <summary>
/// This is the first combo in the basic attack 
/// </summary>
public class BasicAttackComboOne : BasicAttackComboDecorator
{
    
    public BasicAttackComboOne(IBasicAttack wrappedBasicAttack) 
        : base(wrappedBasicAttack)
    {
                
    }

    public override void Execute()
    {
        _wrappedBasicAttack.Execute();
    }

    protected override void SwitchWeapon()
    {

    }
}
