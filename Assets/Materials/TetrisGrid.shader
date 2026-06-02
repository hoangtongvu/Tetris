Shader "Tetris/GridOverlay"
{
    Properties
    {
        // ── Grid dimensions ──────────────────────────────────────────────
        _GridWidth       ("Grid Width  (cells)",      Float)        = 10
        _GridHeight      ("Grid Height (cells)",      Float)        = 20

        // World-space width of a single cell.
        // Cell height is derived as: cellWorldHeight = cellWorldWidth * (gridHeight / gridWidth)
        // so cells remain square regardless of the quad's aspect ratio.
        _CellWorldWidth  ("Cell World Width",         Float)        = 1.0

        // ── Visual ───────────────────────────────────────────────────────
        _LineColor       ("Line Color",               Color)        = (1, 1, 1, 0.18)
        _FillColor       ("Fill Color",               Color)        = (0, 0, 0, 0.06)
        _LineThickness   ("Line Thickness (0-0.5)",   Range(0,0.5)) = 0.035

        // Brighter line every N cells (set to 0 to disable)
        _AccentEvery     ("Accent Every N Cells",     Float)        = 5
        _AccentAlpha     ("Accent Extra Alpha",       Range(0,1))   = 0.30
    }

    SubShader
    {
        Tags
        {
            "Queue"           = "Transparent"
            "RenderType"      = "Transparent"
            "IgnoreProjector" = "True"
        }

        ZWrite Off
        ZTest  LEqual
        Cull   Off
        Blend  SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   3.0

            #include "UnityCG.cginc"

            // ── Uniforms ─────────────────────────────────────────────────
            float  _GridWidth;
            float  _GridHeight;
            float  _CellWorldWidth;

            float4 _LineColor;
            float4 _FillColor;
            float  _LineThickness;

            float  _AccentEvery;
            float  _AccentAlpha;

            // ── Structs ──────────────────────────────────────────────────
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 gridUV     : TEXCOORD0;   // ranges [0,W] x [0,H] in cell units
                float2 rawUV      : TEXCOORD1;   // 0..1 across the quad
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // ── Vertex shader ────────────────────────────────────────────
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionCS = UnityObjectToClipPos(IN.positionOS);
                OUT.rawUV      = IN.uv;
                OUT.gridUV     = IN.uv * float2(_GridWidth, _GridHeight);
                return OUT;
            }

            // ── AA grid line helper ──────────────────────────────────────
            // Returns 0..1 mask: 1 at every integer of 't', soft edge.
            // 'halfThick' is half the line width in the same units as 't'.
            float GridLineMask(float t, float halfThick)
            {
                float d  = abs(frac(t + 0.5) - 0.5);   // dist to nearest integer
                float fw = fwidth(t);                    // ~1 screen pixel in t-space
                return 1.0 - smoothstep(halfThick - fw, halfThick + fw, d);
            }

            // ── Fragment shader ──────────────────────────────────────────
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv   = IN.gridUV;
                float  half = _LineThickness * 0.5;

                // Primary grid lines
                float lx = GridLineMask(uv.x, half);
                float ly = GridLineMask(uv.y, half);
                float primaryMask = max(lx, ly);

                // Accent lines every N cells
                float accentMask = 0.0;
                [branch]
                if (_AccentEvery > 0.5)
                {
                    float2 acUV  = uv / _AccentEvery;
                    float  alx   = GridLineMask(acUV.x, half / _AccentEvery);
                    float  aly   = GridLineMask(acUV.y, half / _AccentEvery);
                    accentMask = max(alx, aly);
                }

                // Combined mask: accent boosts alpha on top of primary lines
                float lineMask = saturate(primaryMask + accentMask * _AccentAlpha);

                // ── Colour assembly ──────────────────────────────────────
                // Base: dim fill across the whole quad
                float4 col = _FillColor;

                // Overlay the line colour
                float4 lc  = _LineColor;
                lc.a      *= lineMask;
                col.rgb    = lerp(col.rgb, lc.rgb, lc.a);
                col.a      = saturate(col.a + lc.a);

                // Hard-clip to quad bounds (in case UVs go slightly outside)
                float2 inside = step(0.0, IN.rawUV) * step(IN.rawUV, 1.0);
                col.a *= inside.x * inside.y;

                return col;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
