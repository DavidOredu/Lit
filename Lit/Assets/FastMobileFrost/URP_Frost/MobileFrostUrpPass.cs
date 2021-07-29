namespace UnityEngine.Rendering.Universal
{
    internal class MobileFrostUrpPass : ScriptableRenderPass
    {
        public Material material;

        private RenderTargetIdentifier source;
        private RenderTargetIdentifier tempCopy = new RenderTargetIdentifier(tempCopyString);

        private readonly string tag;
        private readonly float vignette;
        private readonly float transparency;
        private readonly Texture2D frostTexture;
        private readonly Texture2D normalTexture;

        static readonly int vignetteString = Shader.PropertyToID("_Vignette");
        static readonly int transparencyString = Shader.PropertyToID("_Transparency");
        static readonly int frostString = Shader.PropertyToID("_FrostTex");
        static readonly int normalString = Shader.PropertyToID("_BumpTex");
        static readonly int tempCopyString = Shader.PropertyToID("_TempCopy");

        public MobileFrostUrpPass(RenderPassEvent renderPassEvent, Material material,
            float vignette, float transparency, Texture2D frostTexture, Texture2D normalTexture, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.material = material;
            this.vignette = vignette;
            this.transparency = transparency;
            this.frostTexture = frostTexture;
            this.normalTexture = normalTexture;
            this.tag = tag;
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(tag);
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(tempCopyString, opaqueDesc, FilterMode.Bilinear);
            cmd.CopyTexture(source, tempCopy);
            material.SetTexture(frostString, frostTexture);
            material.SetTexture(normalString, normalTexture);
            material.SetFloat(transparencyString, transparency);
            material.SetFloat(vignetteString, vignette * 2 - 1);
            cmd.Blit(tempCopy, source, material, 0);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempCopyString);
        }
    }
}
