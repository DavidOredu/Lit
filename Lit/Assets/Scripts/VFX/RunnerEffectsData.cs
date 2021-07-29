using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRunnerEffect", menuName = "Effects/Runner Effect")]
public class RunnerEffectsData : ScriptableObject
{
    public Texture2D texture = null;
    public float size;
    [ColorUsage(true, true)]
    public Color color = new Color();
    public Gradient colorOverLife;
    public AnimationCurve sizeOverLife;
    public Vector3 velocityRandomA;
    public Vector3 velocityRandomB;
    public float lifetimeRandomA;
    public float lifetimeRandomB;
    public float radius = 2.3f;
}
