Shader "Feynallein/PulsingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PulsingColor("Pulsing Color", color) = (1, 1, 1, 1)
        _Scale("SCALE", float) = 0
        _Color("Color", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        Tags { "RenderQueue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest LEqual
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            fixed4 _PulsingColor;
            float _Scale;

            float SlowPulse(float scale) {
                float x = _Time.y / scale;
                return sin(x) * sin(x);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_Color, _PulsingColor, SlowPulse(_Scale));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
