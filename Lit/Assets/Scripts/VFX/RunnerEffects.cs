using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[System.Serializable]
public class RunnerEffects
{
    public VisualEffect runnerVFX;
    public DamageForm.DamagerType damagerType;

    public RunnerEffects(VisualEffect runnerVFX, DamageForm.DamagerType damagerType)
    {
        this.runnerVFX = runnerVFX;
        this.damagerType = damagerType;
    }
}
