Shader "Custom/ZombieDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0.5
        _EdgeColor ("Edge Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ZWrite On
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex, _NoiseTex;
            float _DissolveAmount;
            float4 _EdgeColor;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float noise = tex2D(_NoiseTex, i.uv).r;
                if (noise < _DissolveAmount)
                    discard; // Avoids draw and keeps depth

                fixed4 col = tex2D(_MainTex, i.uv);

                // Optional: Edge glow effect
                float edge = smoothstep(_DissolveAmount, _DissolveAmount + 0.05, noise);
                col.rgb += _EdgeColor.rgb * (1.0 - edge);

                return col;
            }
            ENDCG
        }
    }
}
