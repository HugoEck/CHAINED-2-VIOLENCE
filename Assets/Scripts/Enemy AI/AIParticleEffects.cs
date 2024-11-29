using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParticleEffects
{
    ParticleSystem[] bloodSplatter;
    ParticleSystem[] hitEffect;
    public void ActivateBloodParticles(Transform transform)
    {
        foreach (Transform child in transform)
        {
            // Check if the child has the specified tag
            if (child.CompareTag("BloodParticles"))
            {
                // Get the ParticleSystem component from this child
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();

                if (particleSystem != null) // Ensure the ParticleSystem exists
                {
                    particleSystem.Play();
                    break; // If you only want to activate the first matching child
                }
            }
        }
    }
    public void ActivateHitParticles(Transform transform)
    {
        foreach (Transform child in transform)
        {
            // Check if the child has the specified tag
            if (child.CompareTag("HitParticles"))
            {
                // Get the ParticleSystem component from this child
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();

                if (particleSystem != null) // Ensure the ParticleSystem exists
                {
                    particleSystem.Play();
                    break; // If you only want to activate the first matching child
                }
            }
        }
    }
}
