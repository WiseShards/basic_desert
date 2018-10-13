Shader "Custom/Desert" {
    Properties {
        _NoiseTex ("Noise (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _WhiteThreshold("White Threshold", Float) = 0.99
        _MinNoise("Min Noise", Float) = 0.5
        _WarpDiffuse("Warp Diffuse", Float) = 0.5
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque"
            "IgnoreProjector" = "True"
        }

        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            uniform sampler2D _NoiseTex;
            uniform fixed4 _NoiseTex_ST;
            uniform fixed4 _Color;
            uniform fixed _WhiteThreshold;
            uniform fixed _MinNoise;
            uniform fixed _WarpDiffuse;

            struct VertexInput {
                half4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
                fixed3 normal : NORMAL;
            };

            struct VertexOutput {
                fixed2 uv : TEXCOORD0;
                SHADOW_COORDS(1)
                half4 pos : SV_POSITION;
                fixed3 diff : COLOR0;
                fixed3 ambient: COLOR1;
            };

            VertexOutput vert(VertexInput v) {
                VertexOutput o;

                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);

                TRANSFER_SHADOW(o)

                o.pos = UnityObjectToClipPos(v.vertex);

                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

                fixed nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                nl = (nl * _WarpDiffuse) + (1.0 - _WarpDiffuse);
                o.diff = nl * _LightColor0;

                o.ambient = ShadeSH9(fixed4(worldNormal,1));

                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target {
                fixed noise = tex2D(_NoiseTex, i.uv).r;
                noise = lerp(_MinNoise, 1.0, noise);
                fixed w = step(_WhiteThreshold, noise);
                fixed4 color = (_Color * noise) + w;

                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;

                color.rgb *= lighting;
                return color;
            }

            ENDCG  
        }
    }

	FallBack "Mobile/Diffuse"
}