namespace UnityEngine.Rendering.Universal
{
    public class MobileFrostUrp : ScriptableRendererFeature
    {
        [System.Serializable]
        public class MobileFrostLwrpSettings
        {
            public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;

            public Material blitMaterial = null;
            [Range(0, 1)]
            public float Vignette = 1f;
            [Range(0, 1)]
            public float Transparency = 1f;
            public Texture2D FrostTexture;
            public Texture2D NormalTexture;
        }

        public MobileFrostLwrpSettings settings = new MobileFrostLwrpSettings();
        MobileFrostUrpPass mobileFrostLwrpPass;

        public override void Create()
        {
            mobileFrostLwrpPass = new MobileFrostUrpPass(settings.Event, settings.blitMaterial,
                settings.Vignette, settings.Transparency, settings.FrostTexture, settings.NormalTexture, this.name);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            mobileFrostLwrpPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(mobileFrostLwrpPass);
        }
    }
}

