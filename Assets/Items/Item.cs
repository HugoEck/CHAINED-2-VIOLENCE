using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Material legendaryMaterial;
    public Material rareMaterial;
    public Material commonMaterial;

    public MeshRenderer currentRarity;

    public TextMeshPro stat1;
    public TextMeshPro stat2;
    public TextMeshPro stat3;
    public TextMeshPro stat4;
    public TextMeshPro itemName;

    public float attackModifier = 0;
    public float speedModifier = 0;
    public int chainkModifier = 0;
    public float healthkModifier = 0;
    // Start is called before the first frame update
    public void AssignStats(List<string> stats)
    {
        // Reset all stat text fields to empty initially
        stat1.text = "";
        stat2.text = "";
        stat3.text = "";
        stat4.text = "";

        // Fill the stat text fields in order
        for (int i = 0; i < stats.Count; i++)
        {
            switch (i)
            {
                case 0:
                    stat1.text = stats[i].ToString();
                    break;
                case 1:
                    stat2.text = stats[i].ToString();
                    break;
                case 2:
                    stat3.text = stats[i].ToString();
                    break;
                case 3:
                    stat4.text = stats[i].ToString();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < stats.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        stat1.text = stats[i].ToString();
                        break;
                    case 1:
                        stat2.text = stats[i].ToString();
                        break;
                    case 2:
                        stat3.text = stats[i].ToString();
                        break;
                    case 3:
                        stat4.text = stats[i].ToString();
                        break;
                }
            }
        }
    }

    public void AssignName(string name)
    {
        itemName.text = name;
    }
}
