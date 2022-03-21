using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[ExecuteInEditMode]
public class VFXConnector : MonoBehaviour
{
    public RacerAwakening racerAwakening;
    public List<RunnerEffectsData> runnerEffectsData = new List<RunnerEffectsData>();
    public int debugCode;
    public bool useDebugCode;
    // Start is called before the first frame update
    void Start()
    {
        racerAwakening = transform.parent.GetComponent<RacerAwakening>();
        racerAwakening.VFXConnector = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(useDebugCode)
            ChangeVFXProperties(debugCode);
    }
    public void ChangeVFXProperties(int stickmanCode)
    {
        RunnerEffectsData newEffect = runnerEffectsData[stickmanCode];
        racerAwakening.runnerEffects.runnerVFX.SetTexture("_MainTex", newEffect.texture);
        racerAwakening.runnerEffects.runnerVFX.SetFloat("Size", newEffect.size);
        //set color
        racerAwakening.runnerEffects.runnerVFX.SetAnimationCurve("SizeOverLife", newEffect.sizeOverLife);
        racerAwakening.runnerEffects.runnerVFX.SetGradient("ColorOverLife", newEffect.colorOverLife);
        racerAwakening.runnerEffects.runnerVFX.SetGradient("ColorOverLife", newEffect.colorOverLife);
        racerAwakening.runnerEffects.runnerVFX.SetVector3("VelocityRandomA", newEffect.velocityRandomA);
        racerAwakening.runnerEffects.runnerVFX.SetVector3("VelocityRandomB", newEffect.velocityRandomB);
        racerAwakening.runnerEffects.runnerVFX.SetFloat("LifetimeRandomA", newEffect.lifetimeRandomA);
        racerAwakening.runnerEffects.runnerVFX.SetFloat("LifetimeRandomB", newEffect.lifetimeRandomB);
        racerAwakening.runnerEffects.runnerVFX.SetFloat("Radius", newEffect.radius);
        racerAwakening.runnerEffects.runnerVFX.SetVector4("Color", new Vector4(newEffect.color.r, newEffect.color.g, newEffect.color.b, newEffect.color.a));
    }
}
