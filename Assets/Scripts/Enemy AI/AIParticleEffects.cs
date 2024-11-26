using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParticleEffects
{
    ParticleSystem[] bloodSplatter;
    ParticleSystem[] hitEffect;
    public void ActivateBloodParticles(Transform transform)
    {
        if (transform.childCount > 0)
        {

            bloodSplatter = transform.GetComponentsInChildren<ParticleSystem>();
 
            foreach (ParticleSystem particles in bloodSplatter)
            { 
                particles.Play();
            }
        }
    }
    public void ActivateHitParticles(Transform transform)
    {
        if (transform.childCount > 0)
        {
            hitEffect = transform.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particles in hitEffect)
            {
                particles.Play();
            }
        }
    }
}
