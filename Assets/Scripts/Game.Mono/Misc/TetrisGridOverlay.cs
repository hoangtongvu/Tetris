using Game.Mono;
using Reflex.Attributes;
using UnityEngine;

/// <summary>
/// Attach to a Quad (or any flat mesh) that sits just in front of your Tetris
/// play-field.  The script keeps the quad's world size in sync with the shader
/// properties, so the grid lines always align with your game cells.
///
/// Shader property summary
/// ─────────────────────────────────────────────────────
///  _GridWidth       – number of columns  (e.g. 10)
///  _GridHeight      – number of rows     (e.g. 20)
///  _CellWorldWidth  – world-space width of one cell
///                     (the quad is auto-resized to match)
///  _LineColor       – RGBA of grid lines (keep alpha low, ~0.15–0.25)
///  _FillColor       – RGBA of cell interiors (keep alpha very low, ~0.05)
///  _LineThickness   – 0..0.5, thickness in UV cell-space (~0.03 looks good)
///  _AccentEvery     – draw a brighter line every N cells (0 = off)
///  _AccentAlpha     – extra alpha contribution of accent lines
/// </summary>
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class TetrisGridOverlay : MonoBehaviour
{
    [Inject] private BoardConfig boardConfig;

    [Header("Visuals")]
    [ColorUsage(true, false)]
    public Color lineColor = new Color(1f, 1f, 1f, 0.18f);
    [ColorUsage(true, false)]
    public Color fillColor = new Color(0f, 0f, 0f, 0.06f);
    [Range(0f, 0.5f)]
    public float lineThickness = 0.035f;

    [Header("Accent Lines")]
    [Min(0)] public int accentEvery = 5;
    [Range(0f, 1f)]
    public float accentAlpha = 0.30f;

    // ── private ──────────────────────────────────────────────────────────
    static readonly int ID_GridWidth = Shader.PropertyToID("_GridWidth");
    static readonly int ID_GridHeight = Shader.PropertyToID("_GridHeight");
    static readonly int ID_CellWorldWidth = Shader.PropertyToID("_CellWorldWidth");
    static readonly int ID_LineColor = Shader.PropertyToID("_LineColor");
    static readonly int ID_FillColor = Shader.PropertyToID("_FillColor");
    static readonly int ID_LineThickness = Shader.PropertyToID("_LineThickness");
    static readonly int ID_AccentEvery = Shader.PropertyToID("_AccentEvery");
    static readonly int ID_AccentAlpha = Shader.PropertyToID("_AccentAlpha");

    MeshRenderer _mr;
    MaterialPropertyBlock _mpb;

    void OnEnable()
    {
        _mr = GetComponent<MeshRenderer>();
        _mpb = new MaterialPropertyBlock();
        Apply();
    }

    /// <summary>
    /// Push all values to the material property block and resize the quad.
    /// Safe to call every frame; uses MaterialPropertyBlock to avoid material
    /// instancing.
    /// </summary>
    public void Apply()
    {
        if (_mr == null) _mr = GetComponent<MeshRenderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();

        // ── Resize the quad to match the grid in world space ─────────────
        float totalWorldWidth = this.boardConfig.CellWorldSize * this.boardConfig.Width;
        float totalWorldHeight = this.boardConfig.CellWorldSize * this.boardConfig.Height;   // square cells
        transform.localScale = new Vector3(totalWorldWidth, totalWorldHeight, 1f);

        // ── Push shader properties ───────────────────────────────────────
        _mr.GetPropertyBlock(_mpb);

        _mpb.SetFloat(ID_GridWidth, this.boardConfig.Width);
        _mpb.SetFloat(ID_GridHeight, this.boardConfig.Height);
        _mpb.SetFloat(ID_CellWorldWidth, this.boardConfig.CellWorldSize);
        _mpb.SetColor(ID_LineColor, lineColor);
        _mpb.SetColor(ID_FillColor, fillColor);
        _mpb.SetFloat(ID_LineThickness, lineThickness);
        _mpb.SetFloat(ID_AccentEvery, accentEvery);
        _mpb.SetFloat(ID_AccentAlpha, accentAlpha);

        _mr.SetPropertyBlock(_mpb);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draw a wireframe outline of the full grid in the scene view
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        // localScale already encodes the grid size, so draw a unit cube outline
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 0.01f));
    }
#endif
}