Shader "UI/ShineSwipe"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        [MainColor]   _Color   ("Tint", Color) = (1,1,1,1)

        _ShineColor   ("Shine Color", Color) = (1,1,1,1)
        [PowerSlider(1.0)] _ShineIntensity ("Shine Intensity", Range(0,5)) = 1.0
        _ShineAngle   ("Shine Angle (deg)", Range(0,360)) = 45
        _ShineWidth   ("Shine Width", Range(0.001, 1)) = 0.25
        _ShineSoftness("Shine Softness", Range(0.0001, 1)) = 0.15
        _ShineOffset  ("Shine Offset", Range(-1,1)) = -1
        _ShineSpeed   ("Shine Speed (auto)", Range(-5, 5)) = 1.0
        _ShineAlphaMul("Shine Alpha Mult", Range(0,2)) = 1.0

        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask  ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
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
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "UIShine"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;

                float4 _ShineColor;
                float  _ShineIntensity;
                float  _ShineAngle;
                float  _ShineWidth;
                float  _ShineSoftness;
                float  _ShineOffset;
                float  _ShineSpeed;
                float  _ShineAlphaMul;
            CBUFFER_END

            struct appdata {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float4 pos   : SV_POSITION;
                float4 color : COLOR;
                float2 uv    : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = TransformObjectToHClip(v.vertex.xyz);
                o.uv    = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = v.color * _Color;
                return o;
            }

            // soft band around t=0
            float band(float t, float width, float soft)
            {
                float halfW = max(1e-5, width * 0.5);
                float a = smoothstep(-halfW - soft, -halfW + soft, t);
                float b = 1.0 - smoothstep(halfW - soft,  halfW + soft, t);
                return saturate(a * b);
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * i.color;

                float rad = radians(_ShineAngle);
                float2 dir = float2(cos(rad), sin(rad));

                float2 uvC = i.uv - 0.5;

                float autoT = _ShineSpeed * _Time.y * 0.25;
                float slide = fmod(_ShineOffset + autoT + 1.0, 2.0) - 1.0;

                float t = dot(uvC, dir) - slide;
                float m = band(t, _ShineWidth, _ShineSoftness);

                float4 shine = _ShineColor * (_ShineIntensity * m);
                shine.a *= _ShineAlphaMul * m;

                float4 col = baseCol;
                col.rgb += shine.rgb * col.a;
                col.a = saturate(col.a + shine.a);
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
