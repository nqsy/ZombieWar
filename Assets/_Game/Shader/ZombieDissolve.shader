Shader "Custom/ZombieDissolve"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _DissolveAmount("Dissolve Amount", Range(0, 1)) = 0.0
        _EdgeColor("Edge Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        Cull Back
        ZWrite On

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NoiseTex;
        float _DissolveAmount;
        fixed4 _EdgeColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r;

            clip(noise - _DissolveAmount); // Dissolve using clip

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Add edge glow (optional)
            float edge = smoothstep(_DissolveAmount, _DissolveAmount + 0.05, noise);
            c.rgb += _EdgeColor.rgb * (1.0 - edge);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG

        // Separate ShadowCaster pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ColorMask 0

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float _DissolveAmount;

            struct v2f {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)
                o.uv = v.texcoord;
                return o;
            }

            float frag(v2f i) : SV_Target
            {
                float noise = tex2D(_NoiseTex, i.uv).r;
                clip(noise - _DissolveAmount);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
