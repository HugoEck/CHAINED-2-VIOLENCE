
public class BasicAttackComboThree : BasicAttackComboDecorator
{
    public BasicAttackComboThree(IBasicAttack wrappedBasicAttack) 
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
