using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Rendering;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private RenderPipelineAsset lowRenderPipelineAsset;
    [SerializeField] private RenderPipelineAsset mediumRenderPipelineAsset;
    [SerializeField] private RenderPipelineAsset hightRenderPipelineAsset;

    public static GameSettings Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ChangeRenderPipeLine(GraphicsQuality graphicsQuality)
    {
        switch (graphicsQuality)
        {
            case GraphicsQuality.High:
                GraphicsSettings.defaultRenderPipeline = hightRenderPipelineAsset;
                break;
            case GraphicsQuality.Low:
                GraphicsSettings.defaultRenderPipeline = lowRenderPipelineAsset;
                break;
            case GraphicsQuality.Medium :
                GraphicsSettings.defaultRenderPipeline = mediumRenderPipelineAsset;
                break;
        }
    }
}
