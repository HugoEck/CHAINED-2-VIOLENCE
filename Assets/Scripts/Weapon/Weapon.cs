using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Unarmed, // Don't set this for the weapons, it's just default value for the combo manager

        TwoHanded,
        OneHanded,
        ReallyBigTwoHanded,
    }

    public WeaponType currentWeaponType = WeaponType.TwoHanded;
    public ComboAttackSO[] combos;

    public int weaponId;
    public string weaponName;
    public float damage;
    public float durability;

    public float attackRange;
    public float knockback;
    public float stunDuration;
    public void DecreaseDurability()
    {
        durability -= 1;
    }
}
