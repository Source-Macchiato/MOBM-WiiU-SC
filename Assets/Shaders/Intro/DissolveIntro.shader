Shader "Custom/DissolveIntro"
{
    Properties {
        _MainTex ("Texture principale", 2D) = "white" {}
        _DissolveTex ("Texture de dissolution", 2D) = "white" {}
        _Cutoff ("Seuil", Range(0,1)) = 0.0
        _EdgeColor ("Couleur de bord", Color) = (1,1,1,1)
        _EdgeWidth ("Largeur du bord", Range(0,0.1)) = 0.05
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 200

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float _Cutoff;
            float4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float mask = tex2D(_DissolveTex, i.uv).r;
                
                if(mask < _Cutoff)
                    discard;
                if(mask < _Cutoff + _EdgeWidth)
                    col = _EdgeColor;
                return col;
            }
            ENDCG
        }
    }
}
