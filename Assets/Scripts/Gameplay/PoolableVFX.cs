using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableVFX : Poolable
{
    ParticleSystem particles;

    public override void Init()
    {
        base.Init();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Play() => particles.Play();
    
}
