using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[ExecuteInEditMode]
public class VFXConnector : MonoBehaviour
{
    public Racer racer;
    public List<RunnerEffectsData> runnerEffectsData = new List<RunnerEffectsData>();
    public int debugCode;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        racer = transform.parent.GetComponent<Racer>();
        racer.VFXConnector = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(gameManager.currentGameState != GameManager.GameState.InGame)
            ChangeVFXProperties(debugCode);
    }
    public void ChangeVFXProperties(int stickmanCode)
    {
        RunnerEffectsData newEffect = runnerEffectsData[stickmanCode];
        racer.runnerEffects.runnerVFX.SetTexture("_MainTex", newEffect.texture);
        racer.runnerEffects.runnerVFX.SetFloat("Size", newEffect.size);
        //set color
        racer.runnerEffects.runnerVFX.SetAnimationCurve("SizeOverLife", newEffect.sizeOverLife);
        racer.runnerEffects.runnerVFX.SetGradient("ColorOverLife", newEffect.colorOverLife);
        racer.runnerEffects.runnerVFX.SetGradient("ColorOverLife", newEffect.colorOverLife);
        racer.runnerEffects.runnerVFX.SetVector3("VelocityRandomA", newEffect.velocityRandomA);
        racer.runnerEffects.runnerVFX.SetVector3("VelocityRandomB", newEffect.velocityRandomB);
        racer.runnerEffects.runnerVFX.SetFloat("LifetimeRandomA", newEffect.lifetimeRandomA);
        racer.runnerEffects.runnerVFX.SetFloat("LifetimeRandomB", newEffect.lifetimeRandomB);
        racer.runnerEffects.runnerVFX.SetFloat("Radius", newEffect.radius);
        racer.runnerEffects.runnerVFX.SetVector4("Color", new Vector4(newEffect.color.r, newEffect.color.g, newEffect.color.b, newEffect.color.a));
    }
}
