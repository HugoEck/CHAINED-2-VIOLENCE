using UnityEngine;

public class ChainLaserController : MonoBehaviour
{
    public KeyCode activateLaserKey = KeyCode.L; // The key to activate the laser ability

    private void Update()
    {
        if (Input.GetKeyDown(activateLaserKey))
        {
            ActivateLaserOnChain();
        }
    }

    private void ActivateLaserOnChain()
    {
        // Find all objects with the "ChainSegment" tag
        GameObject[] chainSegments = GameObject.FindGameObjectsWithTag("ChainSegment");

        // Loop through all the chain segments and activate their laser ability
        foreach (GameObject segment in chainSegments)
        {
            LaserChain laserScript = segment.GetComponent<LaserChain>();
            if (laserScript != null)
            {
                laserScript.ActivateLaser();
            }
        }
    }
}
