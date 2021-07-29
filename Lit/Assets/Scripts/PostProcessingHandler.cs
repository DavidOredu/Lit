using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

public class PostProcessingHandler : MonoBehaviour
{
    public Volume volume;
    [Header("Vignette")]
    Vignette vignette;
    Bloom bloom;
  //  public VignetteModeParameter modeParameter;
  //  public TextureParameter vignetteMask;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out bloom);
      //  vignette.
        //modeParameter = new VignetteModeParameter();
        //modeParameter.value = VignetteMode.Masked;
        //modeParameter.overrideState = true;
        //vignette.mode = modeParameter;
        
    }

    // Update is called once per frame
    void Update()
    {
     //   vignette.mask = vignetteMask;
    }
}
