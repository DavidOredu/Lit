using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Features_Particle : Features_Object<ParticleSystem>
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        var main = component.main;
        var comp = component.emission;
        main.startColor = uiFeatures.glowMoteParticle.main.startColor;
        main.startSpeed = uiFeatures.glowMoteParticle.main.startSpeed;
        main.startSize = uiFeatures.glowMoteParticle.main.startSize;
        main.startLifetime = uiFeatures.glowMoteParticle.main.startLifetime;
        main.maxParticles = uiFeatures.glowMoteParticle.main.maxParticles;
        comp.rateOverTime = uiFeatures.glowMoteParticle.emission.rateOverTime;
    }
}
