Shader "Custom/UnlitOuterGlow"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 1, 0, 1)
        _GlowThickness ("Glow Thickness", Range(1, 10)) = 5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Cull Front // Ensure the glow appears outside the object
            ZWrite Off // Don't write to the depth buffer
            Blend SrcAlpha OneMinusSrcAlpha // Additive blending for transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _GlowColor;
            float _GlowThickness;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Simulate the glow by expanding the edges of the object
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV = (screenUV - 0.5) * 2.0; // Normalize to [-1, 1]

                float thickness = _GlowThickness / 100.0;
                float glowFactor = smoothstep(1.0 - thickness, 1.0, length(screenUV));

                float4 glowColor = _GlowColor * glowFactor;
                return glowColor; // Render only the glow
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}