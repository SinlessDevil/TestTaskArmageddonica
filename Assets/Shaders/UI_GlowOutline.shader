Shader "UI/GlowOutline"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MainColor]   _Color   ("Tint", Color) = (1,1,1,1)

        _GlowEnabled   ("Enable Glow", Float) = 1
        _GlowColor     ("Glow Color", Color) = (1,1,0,1)
        _GlowSize      ("Glow Size (px)", Range(0,32)) = 6
        _GlowSoftness  ("Glow Softness", Range(0.05,8)) = 2
        _GlowIntensity ("Glow Intensity", Range(0,5)) = 1

        _PulseSpeed    ("Pulse Speed", Range(0,10)) = 0
        _PulseStrength ("Pulse Strength", Range(0,2)) = 0.5
        _PulseKey      ("Pulse Key", Float) = 0 

        _AlphaClip ("Alpha Clip Threshold", Range(0,1)) = 0

        [HideInInspector]_StencilComp     ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil         ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp       ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask       ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags{
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask  [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "UIGlow"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                float4 _Color;

                float  _GlowEnabled;
                float4 _GlowColor;
                float  _GlowSize;
                float  _GlowSoftness;
                float  _GlowIntensity;

                float  _PulseSpeed;
                float  _PulseStrength;
                float  _PulseKey;

                float  _AlphaClip;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = TransformObjectToHClip(v.vertex.xyz);
                o.uv    = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = v.color * _Color;
                return o;
            }
            
            float SampleExpandedAlpha(float2 uv, float radiusPx)
            {
                float2 r = float2(_MainTex_TexelSize.x * radiusPx,
                                  _MainTex_TexelSize.y * radiusPx);

                float a = 0.0;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( 1, 0) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-1, 0) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( 0, 1) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( 0,-1) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + normalize(float2( 1, 1)) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + normalize(float2(-1, 1)) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + normalize(float2( 1,-1)) * r).a;
                a += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + normalize(float2(-1,-1)) * r).a;
                return a * (1.0/8.0);
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * i.color;
                
                if (_AlphaClip > 0)
                    clip(baseCol.a - _AlphaClip);
                
                if (_GlowEnabled < 0.5)
                    return baseCol;
                
                float t = max(0, _Time.y - _PulseKey);
                float pulse = 1.0 + sin(t * _PulseSpeed) * _PulseStrength;
                float expandedA = SampleExpandedAlpha(i.uv, _GlowSize);
                float rim = saturate((expandedA - baseCol.a) * _GlowSoftness);

                float4 glow = _GlowColor * (rim * _GlowIntensity * pulse);
                float4 col = baseCol;
                col.rgb += glow.rgb;
                col.a = saturate(max(col.a, glow.a));
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
