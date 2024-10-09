using UnityEngine;

/// <summary>
/// This class handles all the logic for each individual chain segment in the chain 
/// </summary>
public class ChainSegment : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}


