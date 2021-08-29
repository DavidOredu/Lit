using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[System.Serializable]
public class RunnerEffects
{
    public VisualEffect runnerVFX;

    public RunnerEffects(VisualEffect runnerVFX)
    {
        this.runnerVFX = runnerVFX;
    }
}
