using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Class attributes/ Class")]
public class ClassAttributeSO : ScriptableObject
{
    public PlayerCombat.PlayerClass thisClass;

    public float attackDamage;
    public float maxHP;
    public float movementSpeed;
    public float mass;
    public float knockBack;
    

}
