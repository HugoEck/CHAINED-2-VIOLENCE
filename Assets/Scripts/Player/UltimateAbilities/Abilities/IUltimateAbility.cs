
public interface IUltimateAbility
{

    /// <summary>
    /// Activates the ultimate ability chain (call from UltimateAbilityManager)
    /// </summary>
    public void UseUltimate();

    /// <summary>
    /// Activation logic for ultimate ability chain
    /// </summary>
    public void Activate();

    /// <summary>
    /// Deactivation logic for the ultimate ability chain
    /// </summary>
    public void Deactivate();

    /// <summary>
    /// Update the ultimate ability chain, such as cooldowns
    /// </summary>
    public void UpdateUltimateAttack();
}
