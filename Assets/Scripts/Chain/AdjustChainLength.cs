using Obi;
using UnityEngine;

/// <summary>
/// This script handles the chain upgrade for increasing its length
/// </summary>
public class AdjustChainLength : MonoBehaviour
{
    [Header("Player transforms")]
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;

    public const int AMOUNT_OF_UPGRADES = 5;
    public static int upgradesUsed = 0;

    ObiRopeCursor cursor;
    ObiRope rope;

    public static float currentChainLength;
    void Start()
    {
        cursor = GetComponentInChildren<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
        
    }

    /// <summary>
    /// This update is just for testing the lengthening
    /// </summary>
    void Update()
    {
        currentChainLength = rope.restLength;
        if (Input.GetKeyDown(KeyCode.H))
        {
            IncreaseRopeLength(1);
        }
            
    }
    /// <summary>
    /// call this in the upgrade system for the chain
    /// </summary>
    /// <param name="unitsToLengthenBy"></param>
    public void IncreaseRopeLength(int unitsToLengthenBy)
    {
        if(upgradesUsed <= AMOUNT_OF_UPGRADES)
        {
            cursor.ChangeLength(unitsToLengthenBy);
            upgradesUsed++;

            Debug.Log("CHAIN IS CURRENTLY: " + rope.restLength.ToString() + " Metres long");

            // This code repositions the particles in the chain so that they don't clip through the ground when the chain is upgraded
            for (int i = 0; i < rope.activeParticleCount; i++)
            {
                if (rope.GetParticlePosition(i).y <= player1.transform.position.y || rope.GetParticlePosition(i).y <= player2.transform.position.y)
                {
                    rope.RecalculateRestPositions();
                }
            }
        }
        else
        {
            Debug.Log("Chain is max length");
        }
    }
    public float ReturnCurrentChainLength()
    {
        currentChainLength = rope.restLength;

        return currentChainLength;
    }
}
