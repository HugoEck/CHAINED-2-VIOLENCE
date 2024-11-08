using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIParticleEffects
{
    ParticleSystem[] bloodSplatter;
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
}
