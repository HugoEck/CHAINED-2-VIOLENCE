using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ToolDemo : MonoBehaviour
{
    public NPC_Customization npc;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NPC_Customization.NPCTheme randomTheme = RandomEnumExtensions.GetRandomEnumValue<NPC_Customization.NPCTheme>();
            NPC_Customization.NPCClass randomClass = RandomEnumExtensions.GetRandomEnumValue<NPC_Customization.NPCClass>();

            // Call the Randomize method with the random theme and class
            npc.Randomize(randomTheme, randomClass);

        }
    }
}

public static class RandomEnumExtensions
{
    public static T GetRandomEnumValue<T>()
    {
        Array enumValues = Enum.GetValues(typeof(T));
        return (T)enumValues.GetValue(UnityEngine.Random.Range(0, enumValues.Length));
    }
}

