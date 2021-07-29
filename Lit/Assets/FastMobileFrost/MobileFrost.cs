using UnityEngine;

[ExecuteInEditMode]
public class MobileFrost : MonoBehaviour
{
    [Range(0, 1)]
    public float Vignette = 1f;
    [Range(0, 1)]
    public float Transparency = 1f;

    public Texture2D frost;
    public Texture2D normal;

    static readonly int vignetteString = Shader.PropertyToID("_Vignette");
    static readonly int transparencyString = Shader.PropertyToID("_Transparency");
    static readonly int frostString = Shader.PropertyToID("_FrostTex");
    static readonly int normalString = Shader.PropertyToID("_BumpTex");

    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture(frostString, frost);
        material.SetTexture(normalString, normal);
        material.SetFloat(transparencyString, Transparency);
        material.SetFloat(vignetteString, Vignette * 2 - 1);
        Graphics.Blit(source, destination, material, 0);
    }
}
