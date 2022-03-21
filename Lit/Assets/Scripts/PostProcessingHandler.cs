using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;
[ExecuteInEditMode]
public class PostProcessingHandler : Singleton<PostProcessingHandler>
{
    public ForwardRendererData rendererData;
    public string featureName = null;
    public Volume volume;
    public Vignette vignette;
    public Bloom bloom;
    public ChannelMixer channelMixer;
    public ChromaticAberration chromaticAberration;

    private ScriptableRendererFeature feature;
    private MobileFrostUrp frostFeature;
    private ColorData colorData;

    [Header("FROST SETTINGS")]
    [Range(0, 1)]
    public float VignetteIntensity = 1f;
    [Range(0, 1)]
    public float Transparency = 1f;

    //  public VignetteModeParameter modeParameter;
    //  public TextureParameter vignetteMask;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out channelMixer);
        volume.profile.TryGet(out chromaticAberration);

        TryGetFeature(out feature);

        frostFeature = feature as MobileFrostUrp;
        featureName = frostFeature.name;

        colorData = Resources.Load<ColorData>("ColorData");
      //  vignette.
        //modeParameter = new VignetteModeParameter();
        //modeParameter.value = VignetteMode.Masked;
        //modeParameter.overrideState = true;
        //vignette.mode = modeParameter;
        
    }
    bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        return feature != null;
        // Update is called once per frame
    }
    void Update()
    {
        frostFeature.settings.Vignette = VignetteIntensity;
        frostFeature.settings.Transparency = Transparency;
        rendererData.SetDirty();
     //   vignette.mask = vignetteMask;
    }
    void StopFrostEffect()
    {
        feature.SetActive(false);
        rendererData.SetDirty();
    }
    [ContextMenu("Turn Vignette Red")]
    public void TurnVignetteRed()
    {
        PostProcessingHandler.instance.vignette.color.Override(colorData.red);
    }
    [ContextMenu("Turn Vignette Blue")]
    public void TurnVignetteBlue()
    {
        PostProcessingHandler.instance.vignette.color.Override(colorData.blue);
    }
}
