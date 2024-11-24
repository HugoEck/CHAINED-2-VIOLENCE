using UnityEngine;

[CreateAssetMenu(menuName = "Combo attack/ Normal attack")]
public class ComboAttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;

    [Header("Attributes for current attack")]
    public float damage;
    public float attackRange;
    public float knockback;
    public float stunDuration;

}
