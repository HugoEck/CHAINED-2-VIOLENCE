Shader "Custom/HolographicEffectWithBase"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)  // Opaque base color
        _HoloColor("Holographic Color", Color) = (0, 0.5, 1, 1) // Holographic effect color
        _TimeSpeed("Time Speed", Float) = 1.0          // Speed of animation
        _WaveStrength("Wave Strength", Float) = 0.1   // Intensity of the wave effect
        _Alpha("Holo Transparency", Range(0,1)) = 1.0 // Transparency of the holographic effect
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha // Enable transparency for the holographic layer
        ZWrite On                       // Enable depth writing for the opaque base
        Cull Off                        // Disable face culling (for both sides rendering)

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _BaseColor;
            float4 _HoloColor;
            float _TimeSpeed;
            float _WaveStrength;
            float _Alpha;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Base Color (Opaque Layer)
                fixed4 baseColor = _BaseColor;

                // Holographic Effect
                float2 direction = float2(1, 0); // Horizontal lines
                float linePattern = sin(dot(i.uv, direction) * 100.0 + _Time.y * _TimeSpeed);

                // Circular waves
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float wave = sin(dist * 20.0 - _Time.y * _TimeSpeed) * _WaveStrength;

                // Combine effects
                float finalEffect = saturate(linePattern + wave);

                // Apply Holographic Color and Transparency
                fixed4 holoColor = _HoloColor * finalEffect;
                holoColor.a = _Alpha * finalEffect;

                // Combine Base Color and Holographic Effect
                return lerp(baseColor, holoColor, holoColor.a);
            }
            ENDCG
        }
    }
    FallBack "Standard"
}