using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public float damage;
    public float durability;

    public void Attack()
    {
        durability -= 1;

    }

}
