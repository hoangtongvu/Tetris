using SaintsField;
using System;

namespace Game.UI.Common.UIRendering;

[Serializable]
public struct UIRenderingConfig
{
    public RenderingType RenderingType;
    public RenderingSpace RenderingSpace;

    [FieldShowIf(
        nameof(RenderingType), RenderingType.UGUI,
        nameof(RenderingSpace), RenderingSpace.Overlay)]
    public CanvasAnchorPreset CanvasAnchorPreset;
}