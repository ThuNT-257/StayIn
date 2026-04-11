Shader "Custom/StickerOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BaseColor ("White Base Color", Color) = (1,1,1,1)
        _BaseSize ("White Base Size", Range(0, 0.2)) = 0.05

        _BorderColor ("Black Border Color", Color) = (0,0,0,1)
        _BorderSize ("Black Border Size", Range(0, 0.1)) = 0.01
        
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _BaseColor;
            float _BaseSize;
            fixed4 _BorderColor;
            float _BorderSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            float CheckAlpha(float2 uv, float radius)
            {
                float a = 0;
                float2 offsets[8] = {
                    float2(0, 1), float2(0, -1), float2(1, 0), float2(-1, 0),
                    float2(1, 1), float2(1, -1), float2(-1, 1), float2(-1, -1)
                };
                
                for(int i = 0; i < 8; i++)
                {
                    float2 sampleUV = uv + offsets[i] * radius * _MainTex_TexelSize.xy * 100;
                    a = max(a, tex2D(_MainTex, sampleUV).a);
                }
                return a;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, IN.texcoord);
                
                float borderAlpha = CheckAlpha(IN.texcoord, _BaseSize + _BorderSize);
                fixed4 borderCol = _BorderColor;
                borderCol.a *= borderAlpha;

                float baseAlpha = CheckAlpha(IN.texcoord, _BaseSize);
                fixed4 baseCol = _BaseColor;
                baseCol.a *= baseAlpha;

                fixed4 outCol = mainCol;
                outCol = lerp(baseCol, outCol, mainCol.a);
                outCol = lerp(borderCol, outCol, max(mainCol.a, baseAlpha));

                outCol.rgb *= outCol.a;
                return outCol;
            }
        ENDCG
        }
    }
}