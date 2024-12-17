using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnDamageText : MonoBehaviour
{
    [SerializeField] private GameObject normalDamageText;
    [SerializeField] private GameObject bigDamageText;

    public void Spawn(Vector3 position, float damage)
    {
        if (damage > 10)
        {
            GameObject bigDamage = Instantiate(bigDamageText, position, Quaternion.identity);
            float text = bigDamage.GetComponent<FloatingDamageText>().damage = Mathf.FloorToInt(damage);
        }
        else
        {
            GameObject normalDamage = Instantiate(normalDamageText, position, Quaternion.identity);
            float text = normalDamage.GetComponent<FloatingDamageText>().damage = Mathf.FloorToInt(damage);
        }

        return;
    }
}
