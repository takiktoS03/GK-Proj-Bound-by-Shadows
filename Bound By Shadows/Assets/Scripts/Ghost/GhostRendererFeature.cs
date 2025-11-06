using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;

public class GhostRendererFeature : ScriptableRendererFeature
{
    class GhostBloomPass : ScriptableRenderPass
    {
        private FilteringSettings filter;
        private Material bloomMat;
        private string profilerTag = "Ghost Bloom Pass";
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public GhostBloomPass(Material mat)
        {
            bloomMat = mat;
            filter = new FilteringSettings(RenderQueueRange.all, LayerMask.GetMask("Ghost"));
            tempTexture.Init("_GhostTempTexture");
        }

        public void Setup(RenderTargetIdentifier src)
        {
            source = src;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (bloomMat == null) return;

            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            using (new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
            {
                // Utwórz RendererList (nowe API)
                var drawSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonTransparent);
                drawSettings.overrideMaterial = bloomMat;

                var rendererListDesc = new RendererListDesc(new ShaderTagId("UniversalForward"), renderingData.cullResults, renderingData.cameraData.camera)
                {
                    sortingCriteria = SortingCriteria.CommonTransparent,
                    rendererConfiguration = PerObjectData.None,
                    renderQueueRange = RenderQueueRange.all,
                    layerMask = LayerMask.GetMask("Ghost")
                };

                var rendererList = context.CreateRendererList(rendererListDesc);

                // Renderujemy tylko duchy
                cmd.SetRenderTarget(source);
                cmd.DrawRendererList(rendererList);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    GhostBloomPass ghostPass;
    Material ghostMat;

    public override void Create()
    {
        // 🔹 Utwórz materiał na podstawie Shader Graphu
        ghostMat = CoreUtils.CreateEngineMaterial("Shader Graphs/GhostBloom");

        ghostPass = new GhostBloomPass(ghostMat)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (ghostMat == null)
            return;

        ghostPass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(ghostPass);
    }
}
