using System;
using UnityEngine;
using UE = UnityEngine;
using UnityEngine.Rendering.PostProcessing;
[Serializable]
[PostProcess(typeof(PostProcessColourAdjustRenderer), PostProcessEvent.BeforeStack, "Farhag/Post Process Colour")]
public sealed class PostProcessColourAdjust : PostProcessEffectSettings
{
    public ColorParameter color = new ColorParameter { value = new Color(1f, 1f, 0.6f, 0.5f) };
}

public sealed class PostProcessColourAdjustRenderer : PostProcessEffectRenderer<PostProcessColourAdjust>
{

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Farhag/ColourAdjust"));
        sheet.properties.SetColor("_Color", settings.color);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
