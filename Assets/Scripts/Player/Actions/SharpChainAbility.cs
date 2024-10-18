using UnityEngine;
using System.Collections;

public class SharpChainAbility : PlayerCombat
{
    //public ChainJointSwitcher chainJointSwitcher;  // Reference to the ChainJointSwitcher
    //public float sharpDuration = 5f;               // Duration for how long the chain stays sharp
    //public float sharpDamage = 20f;                // Damage dealt by the sharp chain
    //public Material sharpMaterial;                 // Material for sharp state (optional)
    //public Material normalMaterial;                // Material for normal state (optional)

    //private bool isSharp = false;                  // Flag to check if the chain is sharp

    //// Override UseAbility from the base class to activate the sharp chain ability
    //public override void UseAbility()
    //{
    //    if (!isSharp && chainJointSwitcher != null)
    //    {
    //        isSharp = true;
    //        Debug.Log("Chain is now sharp!");

    //        // Call the ChainJointSwitcher to switch all segments to FixedJoint and align them
    //        chainJointSwitcher.SwitchToFixedJointsAndAlign();

    //        // Optionally, apply sharp material to visually indicate the chain is sharp
    //        ApplySharpMaterial();

    //        // Start coroutine to disable sharpness after the duration
    //        StartCoroutine(DisableSharpnessAfterDuration());
    //    }
    //}

    //// Coroutine to revert the chain back to normal after the sharp duration ends
    //private IEnumerator DisableSharpnessAfterDuration()
    //{
    //    yield return new WaitForSeconds(sharpDuration);  // Wait for the specified duration

    //    if (chainJointSwitcher != null)
    //    {
    //        // Call the ChainJointSwitcher to revert all segments back to HingeJoint
    //        chainJointSwitcher.SwitchToHingeJoints();

    //        // Optionally, revert to normal material
    //        ApplyNormalMaterial();
    //    }

    //    isSharp = false;
    //    Debug.Log("Chain is no longer sharp.");
    //}

    //// Optional: Apply the sharp material to all chain segments to visually show they are sharp
    //private void ApplySharpMaterial()
    //{
    //    if (sharpMaterial != null)
    //    {
    //        foreach (GameObject segment in chainJointSwitcher.chainSegments)
    //        {
    //            Renderer renderer = segment.GetComponent<Renderer>();
    //            if (renderer != null)
    //            {
    //                renderer.material = sharpMaterial;
    //            }
    //        }
    //    }
    //}

    //// Optional: Revert to the normal material for all chain segments
    //private void ApplyNormalMaterial()
    //{
    //    if (normalMaterial != null)
    //    {
    //        foreach (GameObject segment in chainJointSwitcher.chainSegments)
    //        {
    //            Renderer renderer = segment.GetComponent<Renderer>();
    //            if (renderer != null)
    //            {
    //                renderer.material = normalMaterial;
    //            }
    //        }
    //    }
    //}

    //// Collision detection to deal damage when the chain is sharp
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (isSharp)
    //    {
    //        if (collision.collider.CompareTag("Enemy"))
    //        {
    //            BaseManager enemy = collision.collider.GetComponent<BaseManager>();
    //            if (enemy != null)
    //            {
    //                enemy.DealDamageToEnemy(sharpDamage);  // Apply sharp damage to the enemy
    //                Debug.Log("Dealt sharp damage to " + collision.collider.name);
    //            }
    //        }
    //    }
    //}
}
